﻿using BI.Services.History;
using BI.SignalR;
using Common.DatabaseModels.Models.Files;
using Common.DatabaseModels.Models.NoteContent;
using Common.DatabaseModels.Models.NoteContent.FileContent;
using Common.DatabaseModels.Models.NoteContent.TextContent;
using Common.DTO;
using Common.DTO.Notes.FullNoteContent;
using Common.DTO.WebSockets.InnerNote;
using Domain.Commands.NoteInner;
using Domain.Commands.NoteInner.FileContent.Audios;
using Domain.Commands.NoteInner.FileContent.Documents;
using Domain.Commands.NoteInner.FileContent.Photos;
using Domain.Commands.NoteInner.FileContent.Videos;
using Domain.Queries.Permissions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WriteContext.Repositories.NoteContent;

namespace BI.Services.Notes
{
    public class FullNoteContentHandlerCommand : IRequestHandler<SyncNoteStructureCommand, OperationResult<Unit>>
    {

        private readonly BaseNoteContentRepository baseNoteContentRepository;

        private readonly HistoryCacheService historyCacheService;

        private readonly AppSignalRService appSignalRService;

        private readonly TextNotesRepository textNotesRepository;

        private readonly CollectionNoteRepository collectionNoteRepository;

        private readonly IMediator _mediator;

        private readonly CollectionLinkedService collectionLinkedService;

        public FullNoteContentHandlerCommand(
            BaseNoteContentRepository baseNoteContentRepository,
            HistoryCacheService historyCacheService,
            AppSignalRService appSignalRService,
            TextNotesRepository textNotesRepository,
            CollectionNoteRepository collectionNoteRepository,
            IMediator _mediator,
            CollectionLinkedService collectionLinkedService)
        {

            this.historyCacheService = historyCacheService;
            this.appSignalRService = appSignalRService;
            this.textNotesRepository = textNotesRepository;
            this.collectionNoteRepository = collectionNoteRepository;
            this.baseNoteContentRepository = baseNoteContentRepository;
            this._mediator = _mediator;
            this.collectionLinkedService = collectionLinkedService;
        }


        public async Task<OperationResult<Unit>> Handle(SyncNoteStructureCommand request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.UserId);
            var permissions = await _mediator.Send(command);
            var note = permissions.Note;

            List<Guid> unlinkedItemIds = new();
            List<TextNoteDTO> textItemsThatNeedAdd = null;
            List<BaseNoteContentDTO> photosItemsThatNeedAdd = null;
            List<BaseNoteContentDTO> videosItemsThatNeedAdd = null;
            List<BaseNoteContentDTO> audiosItemsThatNeedAdd = null;
            List<BaseNoteContentDTO> documentsItemsThatNeedAdd = null;
            List<UpdateContentPositionWS> positions = null;

            if (permissions.CanWrite)
            {
                var contents = await baseNoteContentRepository.GetWhereAsync(x => x.NoteId == note.Id);
                var contentIds = contents.Select(x => x.Id).ToList();

                if (request.Diffs.RemovedItems != null && request.Diffs.RemovedItems.Any())
                {
                    var removeIds = request.Diffs.RemovedItems.Select(x => x.Id);
                    var contentsToDelete = contents.Where(x => removeIds.Contains(x.Id));

                    var fileContents = contentsToDelete.Where(x => x.ContentTypeId == ContentTypeENUM.Collection).Cast<CollectionNote>();
                    if (fileContents.Any())
                    {
                        unlinkedItemIds = await collectionLinkedService.UnlinkAndRemoveFileItems(fileContents, note.Id, request.UserId);
                    }

                    var textIds = contentsToDelete.Where(x => x.ContentTypeId == ContentTypeENUM.Text).Select(x => x.Id);
                    if (textIds.Any())
                    {
                        unlinkedItemIds.AddRange(textIds);
                    }

                    if (unlinkedItemIds.Any())
                    {
                        contentsToDelete = contents.Where(x => unlinkedItemIds.Contains(x.Id));
                        await baseNoteContentRepository.RemoveRangeAsync(contentsToDelete);
                    }
                }
                if (request.Diffs.NewTextItems != null && request.Diffs.NewTextItems.Any())
                {
                    textItemsThatNeedAdd = request.Diffs.NewTextItems.Where(x => !contentIds.Contains(x.Id)).ToList();
                    var itemsThatAlreadyAdded = request.Diffs.NewTextItems.Where(x => contentIds.Contains(x.Id)).ToList();

                    if (textItemsThatNeedAdd.Any())
                    {
                        var items = textItemsThatNeedAdd.Select(content => GetTextContent(content, note.Id));
                        await textNotesRepository.AddRangeAsync(items);
                    }
                    if (itemsThatAlreadyAdded.Any())
                    {
                        Console.WriteLine("ITEMS TEXTS EXIST");
                    }
                }

                // FILES
                if (request.Diffs.PhotosCollectionItems != null && request.Diffs.PhotosCollectionItems.Any())
                {
                    photosItemsThatNeedAdd = request.Diffs.PhotosCollectionItems.Where(x => !contentIds.Contains(x.Id)).ToList();
                    var itemsThatAlreadyAdded = request.Diffs.PhotosCollectionItems.Where(x => contentIds.Contains(x.Id)).ToList();

                    if (photosItemsThatNeedAdd.Any())
                    {
                        var items = photosItemsThatNeedAdd.Select(x =>
                        {
                            var cont = GetCollectionContent(x, note.Id, FileTypeEnum.Photo);
                            cont.SetMetaDataPhotos("100%", "auto", 2);
                            return cont;
                        });
                        await collectionNoteRepository.AddRangeAsync(items);
                    }
                    if (itemsThatAlreadyAdded.Any())
                    {
                        Console.WriteLine("ITEMS PHOTOS EXIST");
                    }
                }
                if (request.Diffs.AudiosCollectionItems != null && request.Diffs.AudiosCollectionItems.Any())
                {
                    audiosItemsThatNeedAdd = request.Diffs.AudiosCollectionItems.Where(x => !contentIds.Contains(x.Id)).ToList();
                    var itemsThatAlreadyAdded = request.Diffs.AudiosCollectionItems.Where(x => contentIds.Contains(x.Id)).ToList();

                    if (audiosItemsThatNeedAdd.Any())
                    {
                        var items = audiosItemsThatNeedAdd.Select(x => GetCollectionContent(x, note.Id, FileTypeEnum.Audio));
                        await collectionNoteRepository.AddRangeAsync(items);
                    }
                    if (itemsThatAlreadyAdded.Any())
                    {
                        Console.WriteLine("ITEMS AUDIOS EXIST");
                    }
                }
                if (request.Diffs.VideosCollectionItems != null && request.Diffs.VideosCollectionItems.Any())
                {
                    videosItemsThatNeedAdd = request.Diffs.VideosCollectionItems.Where(x => !contentIds.Contains(x.Id)).ToList();
                    var itemsThatAlreadyAdded = request.Diffs.VideosCollectionItems.Where(x => contentIds.Contains(x.Id)).ToList();

                    if (videosItemsThatNeedAdd.Any())
                    {
                        var items = videosItemsThatNeedAdd.Select(x => GetCollectionContent(x, note.Id, FileTypeEnum.Video));
                        await collectionNoteRepository.AddRangeAsync(items);
                    }
                    if (itemsThatAlreadyAdded.Any())
                    {
                        Console.WriteLine("ITEMS VIDEOS EXIST");
                    }
                }
                if (request.Diffs.DocumentsCollectionItems != null && request.Diffs.DocumentsCollectionItems.Any())
                {
                    documentsItemsThatNeedAdd = request.Diffs.DocumentsCollectionItems.Where(x => !contentIds.Contains(x.Id)).ToList();
                    var itemsThatAlreadyAdded = request.Diffs.DocumentsCollectionItems.Where(x => contentIds.Contains(x.Id)).ToList();

                    if (documentsItemsThatNeedAdd.Any())
                    {
                        var items = documentsItemsThatNeedAdd.Select(x => GetCollectionContent(x, note.Id, FileTypeEnum.Document));
                        await collectionNoteRepository.AddRangeAsync(items);
                    }
                    if (itemsThatAlreadyAdded.Any())
                    {
                        Console.WriteLine("ITEMS DOCUMENTS EXIST");
                    }
                }

                if (request.Diffs.Positions != null && request.Diffs.Positions.Any())
                {
                    var updateItems = new List<BaseNoteContent>();
                    foreach(var item in request.Diffs.Positions)
                    {
                       var content = contents.FirstOrDefault(x => x.Id == item.Id);
                       if(content != null)
                       {
                            content.Order = item.Order;
                            updateItems.Add(content);
                       }
                    }
                    if (updateItems.Any())
                    {
                        positions = updateItems.Select(x => new UpdateContentPositionWS(x.Id, x.Order)).ToList();
                        await baseNoteContentRepository.UpdateRangeAsync(updateItems);
                    }
                }

                historyCacheService.UpdateNote(permissions.Note.Id, permissions.Caller.Id, permissions.Author.Email);

                var updates = new UpdateNoteStructureWS()
                {
                    ContentIdsToDelete = unlinkedItemIds,
                    TextContentsToAdd = textItemsThatNeedAdd,
                    PhotoContentsToAdd = photosItemsThatNeedAdd,
                    VideoContentsToAdd = videosItemsThatNeedAdd,
                    DocumentContentsToAdd = documentsItemsThatNeedAdd,
                    AudioContentsToAdd = audiosItemsThatNeedAdd,
                    Positions = positions
                };
                await appSignalRService.UpdateNoteStructure(request.NoteId, permissions.Caller.Email, updates);

                return new OperationResult<Unit>(true, Unit.Value);
            }

            return new OperationResult<Unit>(false, Unit.Value);
        }

        private TextNote GetTextContent(TextNoteDTO textDto, Guid noteId)
        {
            var textDb = new TextNote();

            // UPDATE BASE
            textDb.Id = textDto.Id;
            textDb.Order = textDto.Order;
            textDb.UpdatedAt = textDto.UpdatedAt;

            textDb.NoteId = noteId;

            // UPDATE TEXT
            textDb.NoteTextTypeId = textDto.NoteTextTypeId;
            textDb.HTypeId = textDto.HeadingTypeId;
            textDb.Checked = textDto.Checked;
            textDb.Contents = textDto.Contents;

            return textDb;
        }

        // FILES
        private CollectionNote GetCollectionContent(BaseNoteContentDTO baseContent, Guid noteId, FileTypeEnum fileTypeEnum)
        {
            var content = new CollectionNote(fileTypeEnum);

            // UPDATE BASE
            content.Id = baseContent.Id;
            content.Order = baseContent.Order;
            content.UpdatedAt = baseContent.UpdatedAt;

            content.NoteId = noteId;

            return content;
        }
    }
}
