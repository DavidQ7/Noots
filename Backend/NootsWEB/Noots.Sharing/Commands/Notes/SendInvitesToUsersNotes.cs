﻿using System.ComponentModel.DataAnnotations;
using Common.Attributes;
using Common.CQRS;
using Common.DatabaseModels.Models.Systems;
using Common.DTO;
using MediatR;

namespace Noots.Sharing.Commands.Notes
{
    public class SendInvitesToUsersNotes : BaseCommandEntity, IRequest<OperationResult<Unit>>
    {
        [RequiredListNotEmpty]
        public List<Guid> UserIds { set; get; }

        [ValidationGuid]
        public Guid NoteId { set; get; }

        [RequiredEnumField(ErrorMessage = "Ref type id is required.")]
        public RefTypeENUM RefTypeId { set; get; }

        [Required]
        public bool SendMessage { set; get; }

        public string? Message { set; get; }
    }
}