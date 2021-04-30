﻿using Common.DatabaseModels.models.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DatabaseModels.models.NoteContent
{
    [Table("AudioNote")]
    public class AudioNote : BaseNoteContent
    {
        public string Name { set; get; }
        public Guid AppFileId { get; set; }
        public AppFile AppFile { get; set; }

        public AudioNote()
        {

        }

        public AudioNote(AudioNote entity, AppFile audio, Guid NoteId)
        {
            this.NoteId = NoteId;
            Order = entity.Order;
            this.UpdatedAt = DateTimeOffset.Now;

            Name = entity.Name;

            AppFile = audio;
        }

    }
}