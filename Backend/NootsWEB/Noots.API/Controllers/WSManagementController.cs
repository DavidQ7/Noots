﻿using Common;
using Common.DTO.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noots.SignalrUpdater.Entities;
using Noots.SignalrUpdater.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Noots.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WSManagementController : ControllerBase
    {
        private readonly AppSignalRService appSignalRService;
        private readonly NoteWSUpdateService noteWSUpdateService;
        private readonly FolderWSUpdateService folderWSUpdateService;

        public WSManagementController(AppSignalRService appSignalRService, NoteWSUpdateService noteWSUpdateService, FolderWSUpdateService folderWSUpdateService)
        {
            this.appSignalRService = appSignalRService;
            this.noteWSUpdateService = noteWSUpdateService;
            this.folderWSUpdateService = folderWSUpdateService;
        }


        [HttpPost("connections")]
        public async Task HandleDeadConnections(List<DeadConnectionDTO> connections)
        {
            foreach (var con in connections)
            {
                if (con.NoteIds != null)
                {
                    foreach (var noteId in con.NoteIds)
                    {
                        var userIds = await noteWSUpdateService.GetNotesUserIds(noteId);
                        await appSignalRService.RemoveOnlineUsersNoteAsync(noteId, con.UserIdentifierConnectionId, con.UserId, userIds);
                    }
                }
                if (con.FolderIds != null)
                {
                    foreach (var folderId in con.FolderIds)
                    {
                        var userIds = await folderWSUpdateService.GetFoldersUserIds(folderId);
                        await appSignalRService.RemoveOnlineUsersFolderAsync(folderId, con.UserIdentifierConnectionId, con.UserId, userIds);
                    }
                }
            }
        }

        [HttpPost("ping")]
        [Authorize]
        public async Task Ping(PingDTO ping)
        {
            var userId = this.GetUserId();

            if (!string.IsNullOrEmpty(ping.ConnectionId))
            {
                await appSignalRService.UpdateUpdateStatus(userId, ping.ConnectionId);
            }
        }
    }
}
