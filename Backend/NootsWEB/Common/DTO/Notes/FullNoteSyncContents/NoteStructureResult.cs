﻿using System;
using System.Collections.Generic;

namespace Common.DTO.Notes.FullNoteSyncContents
{
    public class NoteStructureResult
    {
        public List<UpdateIds> UpdateIds { get; set; } = new List<UpdateIds>();

        public List<Guid> RemovedIds { set; get; } = new List<Guid>();

        public NoteStructureResult()
        {

        }
    }
}