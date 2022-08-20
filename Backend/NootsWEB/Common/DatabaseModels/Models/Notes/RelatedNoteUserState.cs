﻿using Common.DatabaseModels.Models.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.DatabaseModels.Models.Notes
{
    [Table(nameof(RelatedNoteUserState), Schema = SchemeConfig.Note)]
    public class RelatedNoteUserState : BaseEntity<int>
    {
        [NotMapped]
        override public int Id { set; get; }

        public int ReletatedNoteInnerNoteId { set; get; }
        public ReletatedNoteToInnerNote ReletatedNoteInnerNote { set; get; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool IsOpened { set; get; }
    }
}