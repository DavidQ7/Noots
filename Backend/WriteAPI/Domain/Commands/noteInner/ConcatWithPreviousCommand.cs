﻿using Common.Attributes;
using Common.DTO.notes.FullNoteContent;
using MediatR;
using System;


namespace Domain.Commands.noteInner
{
    public class ConcatWithPreviousCommand : BaseCommandEntity, IRequest<TextOperationResult<TextNoteDTO>>
    {
        [ValidationGuidAttribute]
        public Guid NoteId { set; get; }
        [ValidationGuidAttribute]
        public Guid ContentId { set; get; }
    }
}
