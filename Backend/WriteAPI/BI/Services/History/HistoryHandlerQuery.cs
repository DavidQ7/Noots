﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BI.Mapping;
using Common.DTO.History;
using Domain.Queries.History;
using Domain.Queries.Permissions;
using MediatR;
using WriteContext.Repositories.Histories;

namespace BI.Services.History
{
    public class HistoryHandlerQuery:
        IRequestHandler<GetNoteHistoriesQuery, List<NoteHistoryDTO>>,
        IRequestHandler<GetNoteSnapshotQuery, NoteHistoryDTOAnswer>
    {

        private readonly IMediator _mediator;
        private readonly NoteSnapshotRepository noteHistoryRepository;
        private readonly AppCustomMapper noteCustomMapper;
        public HistoryHandlerQuery(
            IMediator _mediator, 
            NoteSnapshotRepository noteHistoryRepository,
            AppCustomMapper noteCustomMapper)
        {
            this._mediator = _mediator;
            this.noteHistoryRepository = noteHistoryRepository;
            this.noteCustomMapper = noteCustomMapper;
        }

        public async Task<List<NoteHistoryDTO>> Handle(GetNoteHistoriesQuery request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);

            if (permissions.CanRead)
            {
                var histories = await noteHistoryRepository.GetNoteHistories(request.NoteId);
                return noteCustomMapper.MapHistoriesToHistoriesDto(histories);
            }

            return new List<NoteHistoryDTO>();
        }

        public async Task<NoteHistoryDTOAnswer> Handle(GetNoteSnapshotQuery request, CancellationToken cancellationToken)
        {
            var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.Email);
            var permissions = await _mediator.Send(command);

            if (permissions.CanRead)
            {
                var snapshot = await noteHistoryRepository.GetSnapshot(request.SnapshotId);
                return new NoteHistoryDTOAnswer(true, noteCustomMapper.MapNoteSnapshotToNoteSnapshotDTO(snapshot));
            }

            return new NoteHistoryDTOAnswer(false, null);
        }
    }
}
