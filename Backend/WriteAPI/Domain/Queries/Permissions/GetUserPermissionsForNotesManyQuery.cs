﻿using Common.DTO.Permissions;
using MediatR;
using System;
using System.Collections.Generic;

namespace Domain.Queries.Permissions
{
    public class GetUserPermissionsForNotesManyQuery : BaseQueryEntity, IRequest<List<(Guid noteId, UserPermissionsForNote perm)>>
    {
        public List<Guid> NoteIds { set; get; }

        public GetUserPermissionsForNotesManyQuery(List<Guid> noteIds, string email)
        {
            this.NoteIds = noteIds;
            this.Email = email;
        }
    }
}