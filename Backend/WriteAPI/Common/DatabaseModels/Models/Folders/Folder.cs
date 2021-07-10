﻿using System;
using System.Collections.Generic;
using Common.DatabaseModels.Models.Systems;
using Common.DatabaseModels.Models.Users;

namespace Common.DatabaseModels.Models.Folders
{
    public class Folder : BaseEntity<Guid>
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

        // TODO THIS MUST BE NULLABLE
        public DateTimeOffset DeletedAt { set; get; }
        public DateTimeOffset UpdatedAt { set; get; }
        public DateTimeOffset CreatedAt { set; get; }
        public List<FoldersNotes> FoldersNotes { set; get; }
        public List<UsersOnPrivateFolders> UsersOnPrivateFolders { set; get; }
    }
}