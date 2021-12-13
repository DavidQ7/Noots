﻿using Common.Attributes;
using Common.DTO;
using Common.DTO.Notes.FullNoteContent;
using MediatR;
using System;
using System.Collections.Generic;

namespace Domain.Commands.NoteInner.FileContent.Photos
{
    public class UpdatePhotosContentsCommand : BaseCommandEntity, IRequest<OperationResult<Unit>>
    {
        [ValidationGuid]
        public Guid NoteId { set; get; }

        [RequiredListNotEmptyAttribute]
        public List<PhotosCollectionNoteDTO> Photos { set; get; } = new List<PhotosCollectionNoteDTO>();
    }
}