﻿using Common.DatabaseModels.helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands.notes
{
    public class SetDeleteNoteCommand : BaseCommandEntity, IRequest<Unit>
    {
        public List<string> Ids { set; get; }
        public NotesType NoteType { set; get; }
        public SetDeleteNoteCommand(string email, List<string> ids) : base(email)
        {
            Ids = ids;
        }
    }
}