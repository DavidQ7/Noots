﻿using System;
using Common.Attributes;
using Common.DTO;
using MediatR;

namespace Domain.Commands.NoteInner.FileContent.Audios
{
    public class UnlinkAudiosCollectionCommand : BaseCommandEntity, IRequest<OperationResult<Unit>>
    {
        [ValidationGuid]
        public Guid NoteId { set; get; }

        [ValidationGuid]
        public Guid ContentId { set; get; }

        public UnlinkAudiosCollectionCommand(Guid NoteId, Guid ContentId, string Email)
        {
            this.NoteId = NoteId;
            this.ContentId = ContentId;
            this.Email = Email;
        }
    }
}