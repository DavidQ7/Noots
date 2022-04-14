﻿using Common.DatabaseModels.Models.Notes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Common.DatabaseModels.Models.History
{
    [Table(nameof(CacheNoteHistory), Schema = SchemeConfig.NoteHistory)]
    public class CacheNoteHistory : BaseEntity<Guid>
    {
        [NotMapped]
        public override Guid Id { get; set; }

        public Guid NoteId { set; get; }
        public Note Note { set; get; }

        [Column(TypeName = "jsonb")]
        public HashSet<Guid> UsersThatEditIds { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }
    }
}
