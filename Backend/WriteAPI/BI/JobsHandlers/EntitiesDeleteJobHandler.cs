﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteContext.Repositories.Folders;
using WriteContext.Repositories.Histories;
using WriteContext.Repositories.Labels;
using WriteContext.Repositories.Notes;

namespace BI.JobsHandlers
{
    public class ConfigForEntitesDeliting
    {
        public int LabelsNDays { set; get; } = 5;

        public int FoldersNDays { set; get; } = 5;

        public int NotesNDays { set; get; } = 5;

        public int HistoriesNDays { set; get; } = 30;
    }

    public class EntitiesDeleteJobHandler
    {
        private readonly LabelRepository labelRepostory;

        private readonly NoteRepository noteRepository;

        private readonly FolderRepository folderRepository;

        private readonly ConfigForEntitesDeliting configForEntitesDeliting;

        private readonly NoteSnapshotRepository noteSnapshotRepository;

        public EntitiesDeleteJobHandler(
            LabelRepository labelRepostory,
            NoteRepository noteRepository,
            FolderRepository folderRepository,
            ConfigForEntitesDeliting configForEntitesDeliting,
            NoteSnapshotRepository noteSnapshotRepository)
        {
            this.labelRepostory = labelRepostory;
            this.noteRepository = noteRepository;
            this.folderRepository = folderRepository;
            this.configForEntitesDeliting = configForEntitesDeliting;
            this.noteSnapshotRepository = noteSnapshotRepository;
        }

        public async Task DeleteLabelsHandler()
        {
            try
            {
                Console.WriteLine("Start labels deleting");

                var earliestTimestamp = DateTimeProvider.Time.AddDays(-configForEntitesDeliting.LabelsNDays);

                var labels = await labelRepostory.GetLabelsThatNeedDeleteAfterTime(earliestTimestamp);

                if (labels.Any())
                {
                    Console.WriteLine($"{labels.Count()} labels will be deleted");
                    await labelRepostory.RemoveRangeAsync(labels);
                    Console.WriteLine("Labels was deleted");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task DeleteNotesHandler()
        {
            try
            {
                Console.WriteLine("Start notes deleting");

                var earliestTimestamp = DateTimeProvider.Time.AddDays(-configForEntitesDeliting.NotesNDays);
                var notes = await noteRepository.GetNotesThatNeedDeleteAfterTime(earliestTimestamp);

                if (notes.Any())
                {
                    Console.WriteLine($"{notes.Count()} notes will be deleted");
                    await noteRepository.RemoveRangeAsync(notes);
                    Console.WriteLine("Notes was deleted");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task DeleteFoldersHandler()
        {
            try
            {
                Console.WriteLine("Start folders deleting");

                var earliestTimestamp = DateTimeProvider.Time.AddDays(-configForEntitesDeliting.FoldersNDays);
                var folders = await folderRepository.GetFoldersThatNeedDeleteAfterTime(earliestTimestamp);

                if (folders.Any())
                {
                    Console.WriteLine($"{folders.Count()} folders will be deleted");
                    await folderRepository.RemoveRangeAsync(folders);
                    Console.WriteLine("Folders was deleted");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task DeleteHistoryHandler()
        {
            try
            {
                Console.WriteLine("Start snapshots deleting");

                var earliestTimestamp = DateTimeProvider.Time.AddDays(-configForEntitesDeliting.HistoriesNDays);
                var snapshots = await noteSnapshotRepository.GetSnapshotsThatNeedDeleteAfterTime(earliestTimestamp);

                if (snapshots.Any())
                {
                    Console.WriteLine($"{snapshots.Count()} snapshots will be deleted");
                    await noteSnapshotRepository.RemoveRangeAsync(snapshots);
                    Console.WriteLine("Folders was deleted");
                }
            }catch(Exception ex) {  
                Console.WriteLine(ex.ToString());
            }
        }
    }
}