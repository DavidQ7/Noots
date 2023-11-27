﻿using Common.DatabaseModels.Models.Users.Notifications;
using Common.DTO;
using Common.DTO.WebSockets.Permissions;
using MediatR;
using Notifications.Services;
using Permissions.Impl;
using Permissions.Queries;
using Sharing.Commands.Notes;
using SignalrUpdater.Impl;

namespace Sharing.Handlers.Commands;

public class RemoveUserFromPrivateNotesHandler : IRequestHandler<RemoveUserFromPrivateNotes, OperationResult<Unit>>
{
    private readonly IMediator mediator;
    private readonly AppSignalRService appSignalRHub;
    private readonly NotificationService notificationService;
    private readonly UsersOnPrivateNotesService usersOnPrivateNotesService;

    public RemoveUserFromPrivateNotesHandler(
        IMediator _mediator, 
        AppSignalRService appSignalRHub,
        NotificationService notificationService,
        UsersOnPrivateNotesService usersOnPrivateNotesService)
    {
        mediator = _mediator;
        this.appSignalRHub = appSignalRHub;
        this.notificationService = notificationService;
        this.usersOnPrivateNotesService = usersOnPrivateNotesService;
    }
    
    public async Task<OperationResult<Unit>> Handle(RemoveUserFromPrivateNotes request, CancellationToken cancellationToken)
    {
        var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.UserId);
        var permissions = await mediator.Send(command);

        if (permissions.IsOwner)
        {
            var isRevoked = await usersOnPrivateNotesService.RevokePermissionsNotes(request.PermissionUserId, new List<Guid> { request.NoteId });
            if (isRevoked)
            {
                var updateCommand = new UpdatePermissionNoteWS();
                updateCommand.RevokeIds.Add(request.NoteId);
                await appSignalRHub.UpdatePermissionUserNote(updateCommand, request.PermissionUserId);

                var metadata = new NotificationMetadata { NoteId = request.NoteId, Title = permissions.Note.Title };
                await notificationService.AddAndSendNotification(permissions.Caller.Id, request.PermissionUserId, NotificationMessagesEnum.RemoveUserFromNoteV1, metadata);

                return new OperationResult<Unit>(true, Unit.Value);
            }

            return new OperationResult<Unit>().SetNotFound();
        }
        return new OperationResult<Unit>().SetNoPermissions();
    }
}