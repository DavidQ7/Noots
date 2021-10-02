﻿using Common.DatabaseModels.Models.Files;
using Common.DatabaseModels.Models.Folders;
using Common.DatabaseModels.Models.History;
using Common.DatabaseModels.Models.Labels;
using Common.DatabaseModels.Models.NoteContent;
using Common.DatabaseModels.Models.NoteContent.FileContent;
using Common.DatabaseModels.Models.NoteContent.TextContent;
using Common.DatabaseModels.Models.Notes;
using Common.DatabaseModels.Models.Plan;
using Common.DatabaseModels.Models.Systems;
using Common.DatabaseModels.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace WriteContext
{
    public class WriteContextDB : DbContext
    {
        // USERS & NOTIFICATIONS
        public DbSet<User> Users { get; set; }

        public DbSet<NotificationSetting> NotificationSettings { get; set; }

        public DbSet<PersonalizationSetting> PersonalizationSettings { set; get; }

        public DbSet<SortedByType> SortedByTypes { set; get; }

        public DbSet<Background> Backgrounds { set; get; }

        public DbSet<Notification> Notifications { set; get; }

        public DbSet<UserProfilePhoto> UserProfilePhotos { set; get; }

        // FOLDERS
        public DbSet<Folder> Folders { set; get; }

        public DbSet<FoldersNotes> FoldersNotes { set; get; }

        public DbSet<UsersOnPrivateFolders> UsersOnPrivateFolders { set; get; }

        // LABELS
        public DbSet<Label> Labels { set; get; }

        public DbSet<LabelsNotes> LabelsNotes { set; get; }

        // NOTES
        public DbSet<Note> Notes { set; get; }

        public DbSet<ReletatedNoteToInnerNote> ReletatedNoteToInnerNotes { set; get; }

        public DbSet<UserOnNoteNow> UserOnNoteNow { set; get; }

        public DbSet<UserOnPrivateNotes> UserOnPrivateNotes { set; get; }


        // FILES
        public DbSet<AppFile> Files { set; get; }

        public DbSet<PhotoNoteAppFile> PhotoNoteAppFiles { set; get; }

        public DbSet<VideoNoteAppFile> VideoNoteAppFiles { set; get; }

        public DbSet<DocumentNoteAppFile> DocumentNoteAppFiles { set; get; }

        public DbSet<AudioNoteAppFile> AudioNoteAppFiles { set; get; }

        public DbSet<FileType> FileTypes { set; get; }

        // NOTE CONTENT
        public DbSet<BaseNoteContent> BaseNoteContents { set; get; }

        public DbSet<TextNote> TextNotes { set; get; }

        public DbSet<PhotosCollectionNote> AlbumNotes { set; get; }

        public DbSet<AudiosCollectionNote> AudiosNote { set; get; }

        public DbSet<VideosCollectionNote> VideosNote { set; get; }

        public DbSet<DocumentsCollectionNote> DocumentsNote { set; get; }

        // NOTE HISTORY
        public DbSet<NoteSnapshot> NoteSnapshots { set; get; }

        public DbSet<UserNoteHistoryManyToMany> UserNoteHistoryManyToMany { set; get; }

        // SYSTEMS
        public DbSet<Language> Languages { set; get; }

        public DbSet<Theme> Themes { set; get; }

        public DbSet<FontSize> FontSizes { set; get; }

        public DbSet<RefType> RefTypes { set; get; }

        public DbSet<FolderType> FoldersTypes { set; get; }

        public DbSet<NoteType> NotesTypes { set; get; }

        public DbSet<BillingPlan> BillingPlans { set; get; }

        public DbSet<HType> HTypes { set; get; }

        public DbSet<NoteTextType> NoteTextTypes { set; get; }

        public DbSet<ContentType> ContentTypes { set; get; }

        public WriteContextDB(DbContextOptions<WriteContextDB> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Noots");

            // CONTENT

            modelBuilder.Entity<BaseNoteContent>()
                .HasOne(x => x.NoteSnapshot)
                .WithMany(x => x.Contents)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BaseNoteContent>()
                .HasOne(x => x.Note)
                .WithMany(x => x.Contents)
                .OnDelete(DeleteBehavior.Cascade);

            // USER

            modelBuilder.Entity<User>().HasIndex(x => new { x.Email }).IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(x => x.CurrentBackground)
                .WithOne(z => z.CurrentUserBackground)
                .HasForeignKey<User>(h => h.CurrentBackgroundId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.UserProfilePhoto)
                .WithOne(z => z.User)
                .HasForeignKey<UserProfilePhoto>(h => h.UserId);

            modelBuilder.Entity<UserProfilePhoto>()
                .HasKey(x => x.UserId);

            // PersonalizationSetting

            modelBuilder.Entity<PersonalizationSetting>()
                .Property(b => b.NotesInFolderCount)
                .HasDefaultValue(5);

            modelBuilder.Entity<PersonalizationSetting>()
                .HasOne(x => x.SortedFolderByType)
                .WithMany(x => x.PersonalizationSettingsFolders)
                .HasForeignKey(x => x.SortedFolderByTypeId);

            modelBuilder.Entity<PersonalizationSetting>()
                .HasOne(x => x.SortedNoteByType)
                .WithMany(x => x.PersonalizationSettingsNotes)
                .HasForeignKey(x => x.SortedNoteByTypeId);

            // ----------------------------------------


            modelBuilder.Entity<FileType>()
                .HasMany(x => x.AppFiles)
                .WithOne(x => x.FileType)
                .HasForeignKey(x => x.FileTypeId);

            modelBuilder.Entity<AppFile>()
                .HasOne(x => x.User)
                .WithMany(z => z.Files)
                .HasForeignKey(h => h.UserId);

            modelBuilder.Entity<Note>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<UserOnNoteNow>()
                .HasKey(x => new { x.UserId, x.NoteId });

            modelBuilder.Entity<LabelsNotes>()
                .HasKey(bc => new { bc.NoteId, bc.LabelId });

            modelBuilder.Entity<LabelsNotes>()
                .HasOne(bc => bc.Label)
                .WithMany(b => b.LabelsNotes)
                .HasForeignKey(bc => bc.LabelId);

            modelBuilder.Entity<LabelsNotes>()
                .HasOne(bc => bc.Note)
                .WithMany(c => c.LabelsNotes)
                .HasForeignKey(bc => bc.NoteId);


            modelBuilder.Entity<FoldersNotes>()
                .HasKey(bc => new { bc.NoteId, bc.FolderId });

            modelBuilder.Entity<FoldersNotes>()
                .HasOne(bc => bc.Folder)
                .WithMany(b => b.FoldersNotes)
                .HasForeignKey(bc => bc.FolderId);

            modelBuilder.Entity<FoldersNotes>()
                .HasOne(bc => bc.Note)
                .WithMany(c => c.FoldersNotes)
                .HasForeignKey(bc => bc.NoteId);

            modelBuilder.Entity<UserOnPrivateNotes>()
                .HasKey(bc => new { bc.NoteId, bc.UserId });

            modelBuilder.Entity<UserOnPrivateNotes>()
                .HasOne(bc => bc.Note)
                .WithMany(b => b.UsersOnPrivateNotes)
                .HasForeignKey(bc => bc.NoteId);

            modelBuilder.Entity<UserOnPrivateNotes>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserOnPrivateNotes)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UsersOnPrivateFolders>()
                .HasKey(bc => new { bc.FolderId, bc.UserId });

            modelBuilder.Entity<UsersOnPrivateFolders>()
                .HasOne(bc => bc.Folder)
                .WithMany(b => b.UsersOnPrivateFolders)
                .HasForeignKey(bc => bc.FolderId);

            modelBuilder.Entity<UsersOnPrivateFolders>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UsersOnPrivateFolders)
                .HasForeignKey(bc => bc.UserId);

            // RELATION NOTES

            modelBuilder.Entity<ReletatedNoteToInnerNote>()
                .HasKey(bc => new { bc.NoteId, bc.RelatedNoteId });

            modelBuilder.Entity<ReletatedNoteToInnerNote>()
                .HasOne(bc => bc.Note)
                .WithMany(b => b.ReletatedNoteToInnerNotesFrom)
                .HasForeignKey(bc => bc.NoteId);

            modelBuilder.Entity<ReletatedNoteToInnerNote>()
                .HasOne(bc => bc.RelatedNote)
                .WithMany(b => b.ReletatedNoteToInnerNotesTo)
                .HasForeignKey(bc => bc.RelatedNoteId);


            modelBuilder.Entity<PhotosCollectionNote>()
                .HasMany(p => p.Photos)
                .WithMany(p => p.PhotosCollectionNotes)
                .UsingEntity<PhotoNoteAppFile>(
                    j => j
                        .HasOne(pt => pt.AppFile)
                        .WithMany(t => t.PhotosCollectionNoteAppFiles)
                        .HasForeignKey(pt => pt.AppFileId),
                    j => j
                        .HasOne(pt => pt.PhotosCollectionNote)
                        .WithMany(p => p.PhotoNoteAppFiles)
                        .HasForeignKey(pt => pt.PhotosCollectionNoteId),
                    j =>
                    {
                        j.HasKey(bc => new { bc.PhotosCollectionNoteId, bc.AppFileId });
                    });

            modelBuilder.Entity<AudiosCollectionNote>()
                .HasMany(x => x.Audios)
                .WithMany(x => x.AudiosCollectionNotes)
                .UsingEntity<AudioNoteAppFile>(
                    j => j
                         .HasOne(pt => pt.AppFile)
                         .WithMany(t => t.AudiosCollectionNoteAppFiles)
                         .HasForeignKey(pt => pt.AppFileId),
                    j => j
                         .HasOne(pt => pt.AudiosCollectionNote)
                         .WithMany(p => p.AudioNoteAppFiles)
                         .HasForeignKey(pt => pt.AudiosCollectionNoteId),
                    j =>
                    {
                        j.HasKey(bc => new { bc.AudiosCollectionNoteId, bc.AppFileId });
                    });

            modelBuilder.Entity<VideosCollectionNote>()
                .HasMany(x => x.Videos)
                .WithMany(x => x.VideosCollectionNotes)
                .UsingEntity<VideoNoteAppFile>(
                    j => j
                         .HasOne(pt => pt.AppFile)
                         .WithMany(t => t.VideosCollectionNoteAppFiles)
                         .HasForeignKey(pt => pt.AppFileId),
                    j => j
                         .HasOne(pt => pt.VideosCollectionNote)
                         .WithMany(p => p.VideoNoteAppFiles)
                         .HasForeignKey(pt => pt.VideosCollectionNoteId),
                    j =>
                    {
                        j.HasKey(bc => new { bc.VideosCollectionNoteId, bc.AppFileId });
                    });

            modelBuilder.Entity<DocumentsCollectionNote>()
                .HasMany(x => x.Documents)
                .WithMany(x => x.DocumentsCollectionNotes)
                .UsingEntity<DocumentNoteAppFile>(
                    j => j
                         .HasOne(pt => pt.AppFile)
                         .WithMany(t => t.DocumentsCollectionNoteAppFiles)
                         .HasForeignKey(pt => pt.AppFileId),
                    j => j
                         .HasOne(pt => pt.DocumentsCollectionNote)
                         .WithMany(p => p.DocumentNoteAppFiles)
                         .HasForeignKey(pt => pt.DocumentsCollectionNoteId),
                    j =>
                    {
                        j.HasKey(bc => new { bc.DocumentsCollectionNoteId, bc.AppFileId });
                    });

            modelBuilder.Entity<User>()
                .HasMany(p => p.NoteHistories)
                .WithMany(p => p.Users)
                .UsingEntity<UserNoteHistoryManyToMany>(
                    j => j
                        .HasOne(pt => pt.NoteHistory)
                        .WithMany(t => t.UserHistories)
                        .HasForeignKey(pt => pt.NoteHistoryId),
                    j => j
                        .HasOne(pt => pt.User)
                        .WithMany(p => p.UserHistories)
                        .HasForeignKey(pt => pt.UserId),
                    j =>
                    {
                        j.HasKey(bc => new { bc.UserId, bc.NoteHistoryId });
                    });



            modelBuilder.Entity<Notification>()
                .HasOne(m => m.UserFrom)
                .WithMany(t => t.NotificationsFrom)
                .HasForeignKey(m => m.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(m => m.UserTo)
                .WithMany(t => t.NotificationsTo)
                .HasForeignKey(m => m.UserToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileType>().HasData(
                new FileType { Id = FileTypeEnum.Audio, Name = nameof(FileTypeEnum.Audio) },
                new FileType { Id = FileTypeEnum.Photo, Name = nameof(FileTypeEnum.Photo) },
                new FileType { Id = FileTypeEnum.Video, Name = nameof(FileTypeEnum.Video) },
                new FileType { Id = FileTypeEnum.Document, Name = nameof(FileTypeEnum.Document) }
                );

            modelBuilder.Entity<Language>().HasData(
                new Language { Id = LanguageENUM.English, Name = nameof(LanguageENUM.English) },
                new Language { Id = LanguageENUM.Ukraine, Name = nameof(LanguageENUM.Ukraine) },
                new Language { Id = LanguageENUM.Russian, Name = nameof(LanguageENUM.Russian) });

            modelBuilder.Entity<Theme>().HasData(
                new Theme { Id = ThemeENUM.Dark, Name = nameof(ThemeENUM.Dark) },
                new Theme { Id = ThemeENUM.Light, Name = nameof(ThemeENUM.Light) });

            modelBuilder.Entity<FontSize>().HasData(
                new FontSize { Id = FontSizeENUM.Medium, Name = nameof(FontSizeENUM.Medium) },
                new FontSize { Id = FontSizeENUM.Big, Name = nameof(FontSizeENUM.Big) });



            modelBuilder.Entity<FolderType>().HasData(
                new FolderType { Id = FolderTypeENUM.Private, Name = nameof(FolderTypeENUM.Private) },
                new FolderType { Id = FolderTypeENUM.Shared, Name = nameof(FolderTypeENUM.Shared) },
                new FolderType { Id = FolderTypeENUM.Archived, Name = nameof(FolderTypeENUM.Archived) },
                new FolderType { Id = FolderTypeENUM.Deleted, Name = nameof(FolderTypeENUM.Deleted) });

            modelBuilder.Entity<NoteType>().HasData(
                new NoteType { Id = NoteTypeENUM.Private, Name = nameof(NoteTypeENUM.Private) },
                new NoteType { Id = NoteTypeENUM.Shared, Name = nameof(NoteTypeENUM.Shared) },
                new NoteType { Id = NoteTypeENUM.Archived, Name = nameof(NoteTypeENUM.Archived) },
                new NoteType { Id = NoteTypeENUM.Deleted, Name = nameof(NoteTypeENUM.Deleted) });

            modelBuilder.Entity<RefType>().HasData(
                new RefType { Id = RefTypeENUM.Viewer, Name = nameof(RefTypeENUM.Viewer) },
                new RefType { Id = RefTypeENUM.Editor, Name = nameof(RefTypeENUM.Editor) });

            modelBuilder.Entity<BillingPlan>().HasData(
                new BillingPlan { Id = BillingPlanTypeENUM.Free, Name = nameof(BillingPlanTypeENUM.Free), MaxSize = 1048576000 }, // 1000 MB
                new BillingPlan { Id = BillingPlanTypeENUM.Standart, Name = nameof(BillingPlanTypeENUM.Standart), MaxSize = 5242880000 }, // 5000 MB
                new BillingPlan { Id = BillingPlanTypeENUM.Business, Name = nameof(BillingPlanTypeENUM.Business), MaxSize = 20971520000 }); // 20000 MB


            modelBuilder.Entity<HType>().HasData(
                new HType { Id = HTypeENUM.H1, Name = nameof(HTypeENUM.H1) },
                new HType { Id = HTypeENUM.H2, Name = nameof(HTypeENUM.H2) },
                new HType { Id = HTypeENUM.H3, Name = nameof(HTypeENUM.H3) }
            );

            modelBuilder.Entity<SortedByType>().HasData(
                new SortedByType { Id = SortedByENUM.AscDate, Name = nameof(SortedByENUM.AscDate) },
                new SortedByType { Id = SortedByENUM.DescDate, Name = nameof(SortedByENUM.DescDate) },
                new SortedByType { Id = SortedByENUM.CustomOrder, Name = nameof(SortedByENUM.CustomOrder) }
            );

            modelBuilder.Entity<NoteTextType>().HasData(
                new NoteTextType { Id = NoteTextTypeENUM.Default, Name = nameof(NoteTextTypeENUM.Default) },
                new NoteTextType { Id = NoteTextTypeENUM.Heading, Name = nameof(NoteTextTypeENUM.Heading) },
                new NoteTextType { Id = NoteTextTypeENUM.Dotlist, Name = nameof(NoteTextTypeENUM.Dotlist) },
                new NoteTextType { Id = NoteTextTypeENUM.Numberlist, Name = nameof(NoteTextTypeENUM.Numberlist) },
                new NoteTextType { Id = NoteTextTypeENUM.Checklist, Name = nameof(NoteTextTypeENUM.Checklist) }
            );

            modelBuilder.Entity<ContentType>().HasData(
                new ContentType { Id = ContentTypeENUM.Text, Name = nameof(ContentTypeENUM.Text) },
                new ContentType { Id = ContentTypeENUM.PhotosCollection, Name = nameof(ContentTypeENUM.PhotosCollection) },
                new ContentType { Id = ContentTypeENUM.DocumentsCollection, Name = nameof(ContentTypeENUM.DocumentsCollection) },
                new ContentType { Id = ContentTypeENUM.AudiosCollection, Name = nameof(ContentTypeENUM.AudiosCollection) },
                new ContentType { Id = ContentTypeENUM.VideosCollection, Name = nameof(ContentTypeENUM.VideosCollection) }
             );

        }
    }
}
