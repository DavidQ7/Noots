﻿using Common.DatabaseModels.Models.Notes;
using Common.DTO;
using DatabaseContext.Repositories.Notes;
using MediatR;
using Permissions.Queries;
using Sharing.Commands.Notes;

namespace Sharing.Handlers.Commands;

public class ChangeRefTypeNotesHandler : IRequestHandler<ChangeRefTypeNotes, OperationResult<Unit>>
{
    private readonly IMediator mediator;
    private readonly NoteRepository noteRepository;

    public ChangeRefTypeNotesHandler(IMediator _mediator, NoteRepository noteRepository)
    {
        mediator = _mediator;
        this.noteRepository = noteRepository;
    }
    
    public async Task<OperationResult<Unit>> Handle(ChangeRefTypeNotes request, CancellationToken cancellationToken)
    {
        var command = new GetUserPermissionsForNotesManyQuery(request.Ids, request.UserId);
        var permissions = await mediator.Send(command);
        var isCanEdit = permissions.All(x => x.perm.IsOwner);
        if (isCanEdit)
        {
            foreach (var perm in permissions)
            {
                var note = perm.perm.Note;

                note.RefTypeId = request.RefTypeId;
                note.ToType(NoteTypeENUM.Shared);
                note.SetDateAndVersion();

                await noteRepository.UpdateAsync(note);
            }
            return new OperationResult<Unit>(true, Unit.Value);
        }
        return new OperationResult<Unit>().SetNoPermissions();
    }
}