﻿using System;
using System.Collections.Generic;
using System.Linq;
using BI.Services.Notes;
using Common.DatabaseModels.Models.Folders;
using Common.DatabaseModels.Models.History;
using Common.DatabaseModels.Models.Labels;
using Common.DatabaseModels.Models.NoteContent;
using Common.DatabaseModels.Models.NoteContent.FileContent;
using Common.DatabaseModels.Models.NoteContent.TextContent;
using Common.DatabaseModels.Models.Notes;
using Common.DatabaseModels.Models.Users;
using Common.DTO.App;
using Common.DTO.Folders;
using Common.DTO.History;
using Common.DTO.Labels;
using Common.DTO.Notes;
using Common.DTO.Notes.FullNoteContent;
using Common.DTO.Personalization;
using Common.DTO.Users;


namespace BI.Mapping
{
    public class NoteFolderLabelMapper
    {

        public List<BaseNoteContentDTO> MapContentsToContentsDTO(List<BaseNoteContent> contents)
        {
            if(contents == null)
            {
                return new List<BaseNoteContentDTO>();
            }

            var resultList = new List<BaseNoteContentDTO>();
            
            foreach(var content in contents)
            {
                switch (content)
                {
                    case TextNote tN:
                        {
                            var tNDTO = new TextNoteDTO(tN.Contents, tN.Id, tN.Order, tN.NoteTextTypeId, tN.HTypeId, 
                                tN.Checked, tN.ListId, tN.UpdatedAt);
                            resultList.Add(tNDTO);
                            break;
                        }
                    case PhotosCollectionNote aN:
                        {
                            var photosDTO = aN.Photos.Select(item => new PhotoNoteDTO(item.Id, item.Name, item.PathPhotoSmall, 
                                item.PathPhotoMedium, item.PathPhotoBig, item.UserId, item.CreatedAt)).ToList();
                            var collectionDTO = new PhotosCollectionNoteDTO(photosDTO, aN.Name, aN.Width, aN.Height, aN.Id, aN.Order, aN.CountInRow, aN.UpdatedAt);
                            resultList.Add(collectionDTO);
                            break;
                        }
                    case AudiosCollectionNote playlistNote:
                        {
                            var audiosDTO = playlistNote.Audios.Select(item => new AudioNoteDTO(item.Name, item.Id, item.PathNonPhotoContent, item.UserId, item.MetaData?.SecondsDuration, item.MetaData?.ImagePath, item.CreatedAt)).ToList();
                            var collectionDTO = new AudiosCollectionNoteDTO(playlistNote.Id, playlistNote.Order, playlistNote.UpdatedAt, playlistNote.Name, audiosDTO);                            
                            resultList.Add(collectionDTO);
                            break;
                        }
                    case VideosCollectionNote videoNote:
                        {
                            var videosDTO = videoNote.Videos.Select(item => new VideoNoteDTO(item.Name, item.Id, item.PathNonPhotoContent, item.UserId, item.CreatedAt)).ToList();
                            var collectionDTO = new VideosCollectionNoteDTO(videoNote.Id, videoNote.Order, videoNote.UpdatedAt, videoNote.Name, videosDTO);
                            resultList.Add(collectionDTO);
                            break;
                        }
                    case DocumentsCollectionNote documentNote:
                        {
                            var documentsDTO = documentNote.Documents.Select(item => new DocumentNoteDTO(item.Name, item.PathNonPhotoContent, item.Id, item.UserId, item.CreatedAt)).ToList();
                            var collectionDTO = new DocumentsCollectionNoteDTO(documentNote.Id, documentNote.Order, documentNote.UpdatedAt, documentNote.Name, documentsDTO);
                            resultList.Add(collectionDTO);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Incorrect type");
                        }
                }
            }
            return resultList;
        }

        public NoteTypeDTO MapTypeToTypeDTO(NoteType type)
        {
            return new NoteTypeDTO(type.Id, type.Name);
        }

        public FolderTypeDTO MapTypeToTypeDTO(FolderType type)
        {
            return new FolderTypeDTO(type.Id, type.Name);
        }

        public List<LabelDTO> MapLabelsToLabelsDTO(List<LabelsNotes> labelsNotes)
        {
            var count = labelsNotes.Count();
            return labelsNotes.Select(x => MapLabelToLabelDTO(x, count)).ToList();
        }

        public List<LabelDTO> MapLabelsToLabelsDTO(List<Label> labels)
        {
            return labels.Select(x => MapLabelToLabelDTO(x)).ToList();
        }

        public LabelDTO MapLabelToLabelDTO(LabelsNotes label, int count)
        {
            var lb = label.Label;
            return new LabelDTO()
            {
                Id = lb.Id,
                Name = lb.Name,
                Color = lb.Color,
                CountNotes = count,
                IsDeleted = lb.IsDeleted,
                DeletedAt = lb.DeletedAt,
                CreatedAt = lb.CreatedAt,
                UpdatedAt = lb.UpdatedAt
            };
        }

        public LabelDTO MapLabelToLabelDTO(Label label)
        {
            return new LabelDTO()
            {
                Id = label.Id,
                Name = label.Name,
                Color = label.Color,
                CountNotes = label.LabelsNotes.Count,
                IsDeleted = label.IsDeleted,
                DeletedAt = label.DeletedAt,
                CreatedAt = label.CreatedAt,
                UpdatedAt = label.UpdatedAt
            };
        }

        public RelatedNote MapNoteToRelatedNoteDTO((Note note, bool isOpened) tuple)
        {
            return new RelatedNote()
            {
                Id = tuple.note.Id,
                Color = tuple.note.Color,
                Title = tuple.note.Title,
                Order = tuple.note.Order,
                UserId = tuple.note.UserId,
                IsOpened = tuple.isOpened,
                Labels = tuple.note.LabelsNotes != null ? MapLabelsToLabelsDTO(tuple.note.LabelsNotes?.GetLabelUnDesc()) : null,
                NoteTypeId = tuple.note.NoteTypeId,
                RefTypeId = tuple.note.RefTypeId,
                Contents = GetContentsDTOFromContents(tuple.note.IsLocked, tuple.note.Contents),
                IsLocked = tuple.note.IsLocked,
                DeletedAt = tuple.note.DeletedAt,
                CreatedAt = tuple.note.CreatedAt,
                UpdatedAt = tuple.note.UpdatedAt
            };
        }

        private List<BaseNoteContentDTO> GetContentsDTOFromContents(bool isLocked, List<BaseNoteContent> contents)
        {
            if(!isLocked)
            {
                return MapContentsToContentsDTO(contents).ToList();
            }
            return new List<BaseNoteContentDTO>();
        }

        public SmallNote MapNoteToSmallNoteDTO(Note note)
        {
            return new SmallNote()
            {
                Id = note.Id,
                Color = note.Color,
                Title = note.Title,
                Order = note.Order,
                UserId = note.UserId,
                Labels = note.LabelsNotes != null ? MapLabelsToLabelsDTO(note.LabelsNotes?.GetLabelUnDesc()) : null,
                NoteTypeId = note.NoteTypeId,
                RefTypeId = note.RefTypeId,
                Contents = GetContentsDTOFromContents(note.IsLocked, note.Contents),
                IsLocked = note.IsLocked,
                DeletedAt = note.DeletedAt,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public FullNote MapNoteToFullNote(Note note)
        {
            var _fullNote = new FullNote()
            {
                Id = note.Id,
                Color = note.Color,
                NoteTypeId = note.NoteTypeId,
                RefTypeId = note.RefTypeId,
                Title = note.Title,
                Labels = note.LabelsNotes != null ? MapLabelsToLabelsDTO(note.LabelsNotes?.GetLabelUnDesc()) : null,
                IsLocked = note.IsLocked,
                DeletedAt = note.DeletedAt,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
            return _fullNote;
        }

        public PreviewNoteForSelection MapNoteToPreviewNoteDTO(Note note, IEnumerable<Guid> ids)
        {
            var result =  new PreviewNoteForSelection()
            {
                Id = note.Id,
                Color = note.Color,
                Title = note.Title,
                Order = note.Order,
                UserId = note.UserId,
                Labels = note.LabelsNotes != null ? MapLabelsToLabelsDTO(note.LabelsNotes?.GetLabelUnDesc()) : null,
                NoteTypeId = note.NoteTypeId,
                RefTypeId = note.RefTypeId,
                Contents = GetContentsDTOFromContents(note.IsLocked, note.Contents),
                IsSelected = ids.Contains(note.Id),
                IsLocked = note.IsLocked,
                DeletedAt = note.DeletedAt,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
            var dates = result.Contents.Select(x => x.UpdatedAt);
            dates.Append(result.UpdatedAt);
            result.UpdatedAt = dates.Count() > 0 ? dates.Max() : DateTimeOffset.MinValue; 
            return result;
        }

        public List<PreviewNoteForSelection> MapNotesToPreviewNotesDTO(List<Note> notes, IEnumerable<Guid> ids)
        {
            return notes.Select((note) => MapNoteToPreviewNoteDTO(note, ids)).ToList();
        }

        public List<SmallNote> MapNotesToSmallNotesDTO(IEnumerable<Note> notes)
        {
            return notes.Select(note => MapNoteToSmallNoteDTO(note)).ToList();
        }

        public List<RelatedNote> MapNotesToRelatedNotes(List<ReletatedNoteToInnerNote> notes)
        {
            var resultList = new List<(Note, bool)>();
            notes.ForEach(note => resultList.Add((note.RelatedNote, note.IsOpened)));
            return resultList.Select(tuple => MapNoteToRelatedNoteDTO(tuple)).ToList();
        }

        public List<SmallFolder> MapFoldersToSmallFolders(IEnumerable<Folder> folders)
        {
            return folders.Select(folder => MapFolderToSmallFolder(folder)).ToList();
        }

        public SmallFolder MapFolderToSmallFolder(Folder folder)
        {
            var notes = folder.FoldersNotes?.Select(x => x.Note);
            return new SmallFolder()
            {
                Id = folder.Id,
                Color = folder.Color,
                Order = folder.Order,
                CreatedAt = folder.CreatedAt,
                DeletedAt = folder.DeletedAt,
                UpdatedAt = folder.UpdatedAt,
                Title = folder.Title,
                FolderTypeId = folder.FolderTypeId,
                RefTypeId = folder.RefTypeId,
                PreviewNotes = MapNotesToNotesPreviewInFolder(notes)
            };
        }

        public List<NotePreviewInFolder> MapNotesToNotesPreviewInFolder(IEnumerable<Note> notes)
        {
            return notes?.Select(x => MapNoteToNotePreviewInFolder(x)).ToList();
        }

        public NotePreviewInFolder MapNoteToNotePreviewInFolder(Note note)
        {
            return new NotePreviewInFolder()
            { 
                Title = note.Title
            };
        }

        public IEnumerable<FullFolder> MapFoldersToFullFolders(IEnumerable<Folder> folders)
        {
            return folders.Select(folder => MapFolderToFullFolder(folder));
        }

        public FullFolder MapFolderToFullFolder(Folder folder)
        {
            return new FullFolder()
            {
                Id = folder.Id,
                Color = folder.Color,
                CreatedAt = folder.CreatedAt,
                DeletedAt = folder.DeletedAt,
                UpdatedAt = folder.UpdatedAt,
                Title = folder.Title,
                FolderTypeId = folder.FolderTypeId,
                RefTypeId = folder.RefTypeId
            };
        }


        public UserNoteHistory MapUserToUserNoteHistory(User user)
        {
            return new UserNoteHistory()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhotoId = user.UserProfilePhoto?.AppFileId,
                PhotoPath = user.UserProfilePhoto?.AppFile.GetFromSmallPath ?? user.DefaultPhotoUrl
            };
        }

        public List<UserNoteHistory> MapUsersToUsersNoteHistory(IEnumerable<User> users)
        {
            return users.Select(x => MapUserToUserNoteHistory(x)).ToList();
        }

        public NoteHistoryDTO MapHistoryToHistoryDto(NoteSnapshot historyDTO)
        {
            return new NoteHistoryDTO()
            {
                SnapshotTime = historyDTO.SnapshotTime,
                Users = MapUsersToUsersNoteHistory(historyDTO.Users),
                NoteVersionId = historyDTO.Id
            };
        }

        public List<NoteHistoryDTO> MapHistoriesToHistoriesDto(IEnumerable<NoteSnapshot> histories)
        {
            return histories.Select(x => MapHistoryToHistoryDto(x)).ToList();
        }

        public PersonalizationSettingDTO MapPersonalizationSettingToPersonalizationSettingDTO(PersonalizationSetting pr)
        {
            return new PersonalizationSettingDTO()
            {
                IsViewAudioOnNote = pr.IsViewAudioOnNote,
                IsViewDocumentOnNote = pr.IsViewDocumentOnNote,
                IsViewPhotosOnNote = pr.IsViewPhotosOnNote,
                IsViewTextOnNote = pr.IsViewTextOnNote,
                IsViewVideoOnNote = pr.IsViewVideoOnNote,
                NotesInFolderCount = pr.NotesInFolderCount,
                ContentInNoteCount = pr.ContentInNoteCount,
                SortedNoteByTypeId = pr.SortedNoteByTypeId,
                SortedFolderByTypeId = pr.SortedFolderByTypeId
            };
        }

        public NoteSnapshotDTO MapNoteSnapshotToNoteSnapshotDTO(NoteSnapshot snapshot)
        {
            return new NoteSnapshotDTO()
            {
                Id = snapshot.Id,
                Color = snapshot.Color,
                SnapshotTime = snapshot.SnapshotTime,
                Labels = snapshot.Labels.Select(x => new LabelDTO { Name = x.Name, Color = x.Color }).ToList(),
                NoteId = snapshot.NoteId,
                NoteTypeId = snapshot.NoteTypeId,
                RefTypeId = snapshot.RefTypeId,
                Title = snapshot.Title
            };
        }

    }
}