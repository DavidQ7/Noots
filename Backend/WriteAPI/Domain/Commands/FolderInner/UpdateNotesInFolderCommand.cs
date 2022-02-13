﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Attributes;
using Common.DTO;
using MediatR;

namespace Domain.Commands.FolderInner
{
    public class UpdateNotesInFolderCommand : BaseCommandEntity, IRequest<OperationResult<Unit>>
    {
        [ValidationGuid]
        public Guid FolderId { set; get; }
        
        public List<Guid> NoteIds { set; get; }
    }
}