﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.DatabaseModels.Models.Users
{
    [Table(nameof(PersonalizationSetting), Schema = SchemeConfig.User)]
    public class PersonalizationSetting : BaseEntity<Guid>
    {
        public Guid UserId { set; get; }
        public User User { set; get; }

        public SortedByENUM SortedNoteByTypeId { set; get; }
        public SortedByType SortedNoteByType { set; get; }

        public SortedByENUM SortedFolderByTypeId { set; get; }
        public SortedByType SortedFolderByType { set; get; }


        public int NotesInFolderCount { set; get; }

        public int ContentInNoteCount { set; get; }

        public bool IsViewVideoOnNote { set; get; }

        public bool IsViewAudioOnNote { set; get; }

        public bool IsViewPhotosOnNote { set; get; }

        public bool IsViewTextOnNote { set; get; }

        public bool IsViewDocumentOnNote { set; get; }

        public PersonalizationSetting GetNewFactory(Guid userId)
        {         
            ContentInNoteCount = 10;
            NotesInFolderCount = 5;
            IsViewVideoOnNote = true;
            IsViewAudioOnNote = true;
            IsViewAudioOnNote = true;
            IsViewPhotosOnNote = true;
            IsViewTextOnNote = true;
            IsViewDocumentOnNote = true;
            SortedNoteByTypeId = SortedByENUM.CustomOrder;
            SortedFolderByTypeId = SortedByENUM.CustomOrder;
            UserId = userId;

            return this;
        }

    }
}