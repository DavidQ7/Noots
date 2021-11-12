﻿using System;
using System.Collections.Generic;
using Common.Attributes;
using MediatR;

namespace Domain.Commands.Notes
{
    public class CopyNoteCommand : BaseCommandEntity, IRequest<List<Guid>>
    {
        [RequiredListNotEmptyAttribute]
        public List<Guid> Ids { set; get; }

        public CopyNoteCommand()
        {

        }

        public CopyNoteCommand(string email) : base(email)
        {

        }
    }
}
