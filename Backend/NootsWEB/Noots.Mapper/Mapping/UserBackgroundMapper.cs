﻿using Common.Azure;
using Common.DatabaseModels.Models.Folders;
using Common.DatabaseModels.Models.Notes;
using Common.DatabaseModels.Models.Users;
using Common.DatabaseModels.Models.WS;
using Common.DTO.Backgrounds;
using Common.DTO.Users;

namespace Noots.Mapper.Mapping
{
    public class UserBackgroundMapper : BaseMapper
    {

        public UserBackgroundMapper(AzureConfig azureConfig) : base(azureConfig)
        {
        }

        public OnlineUserOnNote MapToOnlineUserOnNote(User user, Guid userIdentifier)
        {
            return new OnlineUserOnNote
            {
                UserIdentifier = userIdentifier,
                UserId = user.Id,
                Name = user.Name,
                PhotoId = user.UserProfilePhoto?.AppFileId,
                PhotoPath = user.UserProfilePhoto != null ? BuildFilePath(user.Id, user.UserProfilePhoto.AppFile.GetFromSmallPath) : user.DefaultPhotoUrl
            };
        }

        public OnlineUserOnNote MapToOnlineUserOnNote(UserIdentifierConnectionId user)
        {
            return new OnlineUserOnNote
            {
                UserIdentifier = user.Id,
            };
        }


        public InvitedUsersToFoldersOrNote MapToInvitedUsersToFoldersOrNote(UserOnPrivateNotes user)
        {
            return new InvitedUsersToFoldersOrNote
            {
                Id = user.UserId,
                Name = user.User.Name,
                Email = user.User.Email,
                PhotoId = user.User.UserProfilePhoto?.AppFileId,
                PhotoPath = user.User.UserProfilePhoto != null ? BuildFilePath(user.UserId, user.User.UserProfilePhoto.AppFile.GetFromSmallPath) : user.User.DefaultPhotoUrl,
                AccessTypeId = user.AccessTypeId
            };
        }

        public InvitedUsersToFoldersOrNote MapToInvitedUsersToFoldersOrNote(UsersOnPrivateFolders user)
        {
            return new InvitedUsersToFoldersOrNote
            {
                Id = user.UserId,
                Name = user.User.Name,
                Email = user.User.Email,
                PhotoId = user.User.UserProfilePhoto?.AppFileId,
                PhotoPath = user.User.UserProfilePhoto != null ? BuildFilePath(user.UserId, user.User.UserProfilePhoto.AppFile.GetFromSmallPath) : user.User.DefaultPhotoUrl,
                AccessTypeId = user.AccessTypeId
            };
        }

        public BackgroundDTO MapToBackgroundDTO(Background background)
        {
            return new BackgroundDTO
            {
                Id = background.Id,
                PhotoId = background.FileId,
                PhotoPath = BuildFilePath(background.UserId, background.File.GetFromBigPath)
            };
        }

        public Background MapToBackground(BackgroundDTO background)
        {
            return new Background
            {
                Id = background.Id,
                FileId = background.PhotoId
            };
        }
    }
}