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

public class SendInvitesToUsersNotesHandler : IRequestHandler<SendInvitesToUsersNotes, OperationResult<Unit>>
{
    private readonly UsersOnPrivateNotesService usersOnPrivateNotesService;
    private readonly IMediator mediator;
    private readonly AppSignalRService appSignalRHub;
    private readonly NotificationService notificationService;

    public SendInvitesToUsersNotesHandler(
        UsersOnPrivateNotesService usersOnPrivateNotesService,
        IMediator _mediator,
        AppSignalRService appSignalRHub,
        NotificationService notificationService)
    {
        this.usersOnPrivateNotesService = usersOnPrivateNotesService;
        mediator = _mediator;
        this.appSignalRHub = appSignalRHub;
        this.notificationService = notificationService;
    }
    
    public async Task<OperationResult<Unit>> Handle(SendInvitesToUsersNotes request, CancellationToken cancellationToken)
    {
        var command = new GetUserPermissionsForNoteQuery(request.NoteId, request.UserId);
        var permissions = await mediator.Send(command);

        if (permissions.IsOwner)
        {
            await usersOnPrivateNotesService.AddPermissionsAsync(request.NoteId, request.RefTypeId, request.UserIds);

            var updateCommand = new UpdatePermissionNoteWS();
            updateCommand.IdsToAdd.Add(request.NoteId);
            foreach (var userId in request.UserIds)
            {
                await appSignalRHub.UpdatePermissionUserNote(updateCommand, userId);
            }

            // NOTIFICATIONS
            var metadata = new NotificationMetadata { NoteId = request.NoteId, Title = permissions.Note.Title };
            await notificationService.AddAndSendNotificationsAsync(permissions.Caller.Id, request.UserIds, NotificationMessagesEnum.SentInvitesToNoteV1, metadata);

            return new OperationResult<Unit>(true, Unit.Value);
        }

        return new OperationResult<Unit>().SetNoPermissions();
    }
}