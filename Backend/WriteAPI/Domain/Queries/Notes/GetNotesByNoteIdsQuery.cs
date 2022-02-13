﻿using Common.DTO.Notes;
using Common.DTO.Personalization;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Attributes;
using Common.DTO;

namespace Domain.Queries.Notes
{
    public class GetNotesByNoteIdsQuery : BaseQueryEntity, IRequest<OperationResult<List<SmallNote>>>
    {
        [RequiredListNotEmptyAttribute]
        public List<Guid> NoteIds { set; get; }

        [Required]
        public PersonalizationSettingDTO Settings { set; get; }

        public GetNotesByNoteIdsQuery(string email, List<Guid> noteIds, PersonalizationSettingDTO settings)
            : base(email)
        {
            this.Settings = settings;
            this.NoteIds = noteIds;
        }
    }
}