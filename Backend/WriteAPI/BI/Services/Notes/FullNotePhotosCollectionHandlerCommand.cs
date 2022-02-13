﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BI.Helpers;
using BI.Services.History;
using BI.SignalR;
using Common;
using Common.DatabaseModels.Models.NoteContent.FileContent;
using Common.DTO;
using Common.DTO.Notes.FullNoteContent;
using Common.DTO.WebSockets.InnerNote;
using Domain.Commands.Files;
using Domain.Commands.NoteInner.FileContent.Photos;
using Domain.Queries.Permissions;
using MediatR;
using WriteContext.Repositories.Files;
using WriteContext.Repositories.NoteContent;

namespace BI.Services.Notes
{
    public class FullNotePhotosCollectionHandlerCommand : FullNoteBaseCollection,
        IRequestHandler<UnlinkPhotosCollectionCommand, OperationResult<Unit>>,
        IRequestHandler<RemovePhotosFromCollectionCommand, OperationResult<Unit>>,
        IRequestHandler<UpdatePhotosCollectionInfoCommand, OperationResult<Unit>>,
        IRequestHandler<TransformToPhotosCollectionCommand, OperationResult<PhotosCollectionNoteDTO>>,
        IRequestHandler<AddPhotosToCollectionCommand, OperationResult<Unit>>
    {

        private readonly IMediator _mediator;

        private readonly BaseNoteContentRepository baseNoteContentRepository;

        private readonly PhotosCollectionNoteRepository photosCollectionNoteRepository;

        private readonly PhotoNoteAppFileRepository photoNoteAppFileRepository;

        private readonly HistoryCacheService historyCacheService;

        private readonly AppSignalRService appSignalRService;

        public FullNotePhotosCollectionHandlerCommand(
            IMediator _mediator,
            BaseNoteContentRepository baseNoteContentRepository,
            FileRepository fileRepository,
            AppFileUploadInfoRepository appFileUploadInfoRepository,
            PhotosCollectionNoteRepository photosCollectionNoteRepository,
            PhotoNoteAppFileRepository photoNoteAppFileRepository,
            HistoryCacheService historyCacheService,
            AppSignalRService appSignalRService) : base(appFileUploadInfoRepository, fileRepository)
        {
            this._mediator = _mediator;
            this.baseNoteContentRepository = baseNoteContentRepository;
            this.photosCollectionNoteRepository = photosCollectionNoteRepository;
            this.photoNoteAppFileRepository = photoNoteAppFileRepository;
            this.historyCacheService = historyCacheService;
            this.appSignalRService = appSignalRService;
        }


        public async Task<OperationResult<Unit>> Handle(UnlinkPhotosCollectionCommand request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);
            var note = permissions.Note;

            if (permissions.CanWrite)
            {
                var photos = await photoNoteAppFileRepository.GetWhereAsync(x => x.PhotosCollectionNoteId == request.ContentId);

                if (photos.Any())
                {
                    var ids = photos.Select(x => x.AppFileId).ToArray();
                    await MarkAsUnlinked(ids);

                    return new OperationResult<Unit>(success: true, Unit.Value);
                }
                else
                {
                    return new OperationResult<Unit>(success: true, Unit.Value, OperationResultAdditionalInfo.NoAnyFile);
                }
            }

            return new OperationResult<Unit>().SetNoPermissions();
        }

        public async Task<OperationResult<Unit>> Handle(RemovePhotosFromCollectionCommand request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);
            var note = permissions.Note;

            if (permissions.CanWrite)
            {
                var collection = await photosCollectionNoteRepository.FirstOrDefaultAsync(x => x.Id == request.ContentId);
                var collectionItems = await photoNoteAppFileRepository.GetWhereAsync(x => request.FileIds.Contains(x.AppFileId));
                if (collection != null && collectionItems != null && collectionItems.Any())
                {
                    await photoNoteAppFileRepository.RemoveRangeAsync(collectionItems);

                    var idsToUnlink = collectionItems.Select(x => x.AppFileId);
                    await MarkAsUnlinked(idsToUnlink.ToArray());

                    collection.UpdatedAt = DateTimeProvider.Time;
                    await photosCollectionNoteRepository.UpdateAsync(collection);

                    historyCacheService.UpdateNote(permissions.Note.Id, permissions.User.Id, permissions.Author.Email);

                    var updates = new UpdatePhotosCollectionWS(request.ContentId, UpdateOperationEnum.DeleteCollectionItems, collection.UpdatedAt) 
                    { 
                        CollectionItemIds = idsToUnlink
                    };
                    await appSignalRService.UpdatePhotosCollection(request.NoteId, permissions.User.Email, updates);

                    return new OperationResult<Unit>(success: true, Unit.Value);
                }

                return new OperationResult<Unit>().SetNotFound();
            }

            return new OperationResult<Unit>().SetNoPermissions();
        }


        public async Task<OperationResult<Unit>> Handle(UpdatePhotosCollectionInfoCommand request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);

            if (permissions.CanWrite)
            {
                var collection = await photosCollectionNoteRepository.FirstOrDefaultAsync(x => x.Id == request.ContentId);

                if (collection != null)
                {
                    collection.Height = request.Height;
                    collection.Width = request.Width;
                    collection.CountInRow = request.Count;
                    collection.Name = request.Name;

                    collection.UpdatedAt = DateTimeProvider.Time;
                    await baseNoteContentRepository.UpdateAsync(collection);

                    historyCacheService.UpdateNote(permissions.Note.Id, permissions.User.Id, permissions.Author.Email);

                    var updates = new UpdatePhotosCollectionWS(request.ContentId, UpdateOperationEnum.Update, collection.UpdatedAt)
                    {
                        Name = request.Name,
                        CountInRow = request.Count,
                        Height = request.Height,
                        Width = request.Width
                    };
                    await appSignalRService.UpdatePhotosCollection(request.NoteId, permissions.User.Email, updates);

                    return new OperationResult<Unit>(success: true, Unit.Value);
                }

                return new OperationResult<Unit>().SetNotFound();
            }

            return new OperationResult<Unit>().SetNoPermissions();
        }

        public async Task<OperationResult<PhotosCollectionNoteDTO>> Handle(TransformToPhotosCollectionCommand request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);

            if (permissions.CanWrite)
            {
                var contentForRemove = await baseNoteContentRepository.FirstOrDefaultAsync(x => x.Id == request.ContentId);

                if (contentForRemove == null)
                {
                    return new OperationResult<PhotosCollectionNoteDTO>().SetNotFound();
                }

                using var transaction = await baseNoteContentRepository.context.Database.BeginTransactionAsync();

                try
                {
                    await baseNoteContentRepository.RemoveAsync(contentForRemove);

                    var photosCollection = new PhotosCollectionNote()
                    {
                        NoteId = request.NoteId,
                        Order = contentForRemove.Order,
                        CountInRow = 2,
                        Width = "100%",
                        Height = "auto",
                    };

                    await photosCollectionNoteRepository.AddAsync(photosCollection);

                    await transaction.CommitAsync();

                    var result = new PhotosCollectionNoteDTO(null, photosCollection.Name, photosCollection.Width, photosCollection.Height,
                        photosCollection.Id, photosCollection.Order, photosCollection.CountInRow, photosCollection.UpdatedAt);

                    historyCacheService.UpdateNote(permissions.Note.Id, permissions.User.Id, permissions.Author.Email);

                    var updates = new UpdatePhotosCollectionWS(request.ContentId, UpdateOperationEnum.Transform, photosCollection.UpdatedAt)
                    {
                        Collection = result
                    };
                    await appSignalRService.UpdatePhotosCollection(request.NoteId, permissions.User.Email, updates);

                    return new OperationResult<PhotosCollectionNoteDTO>(success: true, result);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(e);
                }
            }

            return new OperationResult<PhotosCollectionNoteDTO>().SetNoPermissions();
        }


        public async Task<OperationResult<Unit>> Handle(AddPhotosToCollectionCommand request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);

            if (permissions.CanWrite)
            {
                var collection = await photosCollectionNoteRepository.FirstOrDefaultAsync(x => x.Id == request.ContentId);
                if (collection != null)
                {
                    var existCollectionItems = await photoNoteAppFileRepository.GetWhereAsync(x => request.FileIds.Contains(x.AppFileId));
                    var existCollectionItemsIds = existCollectionItems.Select(x => x.AppFileId);

                    var collectionItems = request.FileIds.Except(existCollectionItemsIds).Select(id => new PhotoNoteAppFile { AppFileId = id, PhotosCollectionNoteId = collection.Id });
                    await photoNoteAppFileRepository.AddRangeAsync(collectionItems);

                    var idsToLink = collectionItems.Select(x => x.AppFileId);
                    await MarkAsLinked(idsToLink.ToArray());

                    collection.UpdatedAt = DateTimeProvider.Time;
                    await photosCollectionNoteRepository.UpdateAsync(collection);

                    historyCacheService.UpdateNote(permissions.Note.Id, permissions.User.Id, permissions.Author.Email);

                    var updates = new UpdatePhotosCollectionWS(request.ContentId, UpdateOperationEnum.AddCollectionItems, collection.UpdatedAt)
                    {
                        CollectionItemIds = idsToLink
                    };
                    await appSignalRService.UpdatePhotosCollection(request.NoteId, permissions.User.Email, updates);

                    return new OperationResult<Unit>(success: true, Unit.Value);
                }

                return new OperationResult<Unit>().SetNotFound();
            }

            return new OperationResult<Unit>().SetNoPermissions();
        }
    }
}