﻿using Common.DatabaseModels.Models.Folders;
using Common.DatabaseModels.Models.Users.Notifications;
using Common.DTO.WebSockets.Permissions;
using MediatR;
using Noots.DatabaseContext.Repositories.Folders;
using Noots.Notifications.Services;
using Noots.Permissions.Queries;
using Noots.Sharing.Commands.Folders;
using Noots.SignalrUpdater.Impl;

namespace Noots.Sharing.Handlers.Commands;

public class SendInvitesToUsersFoldersHandler: IRequestHandler<SendInvitesToUsersFolders, Unit>
{
    private readonly IMediator mediator;
    private readonly UsersOnPrivateFoldersRepository usersOnPrivateFoldersRepository;
    private readonly NotificationService notificationService;
    private readonly AppSignalRService appSignalRHub;

    public SendInvitesToUsersFoldersHandler(
        IMediator _mediator, 
        UsersOnPrivateFoldersRepository usersOnPrivateFoldersRepository,
        NotificationService notificationService,
        AppSignalRService appSignalRHub)
    {
        mediator = _mediator;
        this.usersOnPrivateFoldersRepository = usersOnPrivateFoldersRepository;
        this.notificationService = notificationService;
        this.appSignalRHub = appSignalRHub;
    }
    
    public async Task<Unit> Handle(SendInvitesToUsersFolders request, CancellationToken cancellationToken)
    {
        var command = new GetUserPermissionsForFolderQuery(request.FolderId, request.UserId);
        var permissions = await mediator.Send(command);

        if (permissions.IsOwner)
        {
            var permissionsRequests = request.UserIds.Select(userId => new UsersOnPrivateFolders()
            {
                AccessTypeId = request.RefTypeId,
                FolderId = request.FolderId,
                UserId = userId
            }).ToList();

            await usersOnPrivateFoldersRepository.AddRangeAsync(permissionsRequests);

            var updateCommand = new UpdatePermissionFolderWS();
            updateCommand.IdsToAdd.Add(request.FolderId);
            foreach (var userId in request.UserIds)
            {
                await appSignalRHub.UpdatePermissionUserFolder(updateCommand, userId);
            }

            // NOTIFICATIONS
            var metadata = new NotificationMetadata { FolderId = request.FolderId, Title = permissions.Folder.Title };
            await notificationService.AddAndSendNotificationsAsync(permissions.Caller.Id, request.UserIds, NotificationMessagesEnum.SentInvitesToFolderV1, metadata, request.Message);
        }

        return Unit.Value;
    }
}