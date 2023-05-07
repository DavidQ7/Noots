﻿using Common.DTO;
using MediatR;
using Noots.Permissions.Queries;
using Noots.SignalrUpdater.Entities;
using Noots.SignalrUpdater.Impl;
using System.Threading;
using System.Threading.Tasks;

namespace BI.Services.Notes.Interaction;

public class UpdateCursorCommandHandler : IRequestHandler<UpdateCursorCommand, OperationResult<Unit>>
{
    private readonly IMediator mediator;
    private readonly AppSignalRService appSignalRHub;

    public UpdateCursorCommandHandler(IMediator _mediator, AppSignalRService appSignalRHub)
    {
        mediator = _mediator;
        this.appSignalRHub = appSignalRHub;
    }

    public async Task<OperationResult<Unit>> Handle(UpdateCursorCommand request, CancellationToken cancellationToken)
    {
        if(request.Cursor == null) return new OperationResult<Unit>().SetAnotherError();

        var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.UserId);
        var permissions = await mediator.Send(command);

        if (!permissions.CanRead) return new OperationResult<Unit>().SetNoPermissions();

        var cursor = request.Cursor;
        var updates = new UpdateCursorWS(
            cursor.EntityId, 
            (CursorTypeWS)cursor.Type, 
            cursor.StartCursor, 
            cursor.EndCursor, 
            cursor.Color,
            cursor.ItemId,
            request.NoteId,
            permissions.Caller.Id);

        await appSignalRHub.UpdateUserNoteCursor(request.NoteId, updates);

        return new OperationResult<Unit>(true, Unit.Value);
    }
}