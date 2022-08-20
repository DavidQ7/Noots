﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Common.DatabaseModels.Models.Systems;
using Common.DatabaseModels.Models.Users;
using Common.Interfaces;

namespace Common.DatabaseModels.Models.Folders
{
    [Table(nameof(Folder), Schema = SchemeConfig.Folder)]
    public class Folder : BaseEntity<Guid>, IDateCreator, IDateUpdater, IDateDeleter
    {
        public FolderTypeENUM FolderTypeId { set; get; }
        public FolderType FolderType { set; get; }

        public RefTypeENUM RefTypeId { set; get; }
        public RefType RefType { set; get; }

        public string Title { set; get; }
        public string Color { set; get; }
        public int Order { set; get; }
        public Guid UserId { set; get; }
        public User User { set; get; }


        public List<FoldersNotes> FoldersNotes { set; get; }
        public List<UsersOnPrivateFolders> UsersOnPrivateFolders { set; get; }

        public DateTimeOffset? DeletedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void ToType(FolderTypeENUM folderTypeId, DateTimeOffset? deletedAt = null)
        {
            DeletedAt = deletedAt;
            FolderTypeId = folderTypeId;
            UpdatedAt = DateTimeProvider.Time;
        }

        public bool IsShared() => FolderTypeId == FolderTypeENUM.Shared;
    }
}