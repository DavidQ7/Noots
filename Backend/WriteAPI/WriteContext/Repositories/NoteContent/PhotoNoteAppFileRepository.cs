﻿using Common.DatabaseModels.Models.NoteContent.FileContent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WriteContext.GenericRepositories;

namespace WriteContext.Repositories.NoteContent
{
    public class PhotoNoteAppFileRepository : Repository<PhotoNoteAppFile, Guid>
    {
        public PhotoNoteAppFileRepository(WriteContextDB contextDB)
        : base(contextDB)
        {
        }

        public Task<List<Guid>> GetFileIdsThatExist(params Guid[] ids)
        {
            return entities.Where(x => ids.Contains(x.AppFileId)).Select(x => x.AppFileId).ToListAsync();
        }
    }
}
