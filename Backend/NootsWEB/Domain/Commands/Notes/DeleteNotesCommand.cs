﻿using System;
using System.Collections.Generic;
using Common.Attributes;
using Common.CQRS;
using Common.DTO;
using MediatR;

namespace Domain.Commands.Notes
{
    public class DeleteNotesCommand : BaseCommandEntity, IRequest<OperationResult<Unit>>
    {
        [RequiredListNotEmptyAttribute]
        public List<Guid> Ids { set; get; }

        public DeleteNotesCommand(Guid userId) : base(userId)
        {

        }
    }
}