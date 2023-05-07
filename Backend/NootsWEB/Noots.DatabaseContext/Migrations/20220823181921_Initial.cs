﻿using System;
using System.Collections.Generic;
using Common.DatabaseModels.Models.Files;
using Common.DatabaseModels.Models.History;
using Common.DatabaseModels.Models.History.Contents;
using Common.DatabaseModels.Models.NoteContent.FileContent;
using Common.DatabaseModels.Models.NoteContent.TextContent.TextBlockElements;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Noots.DatabaseContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "file");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.EnsureSchema(
                name: "note_content");

            migrationBuilder.EnsureSchema(
                name: "note_history");

            migrationBuilder.EnsureSchema(
                name: "folder");

            migrationBuilder.EnsureSchema(
                name: "noots_systems");

            migrationBuilder.EnsureSchema(
                name: "label");

            migrationBuilder.EnsureSchema(
                name: "note");

            migrationBuilder.EnsureSchema(
                name: "ws");

            migrationBuilder.CreateTable(
                name: "BillingPlan",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MaxSize = table.Column<long>(type: "bigint", nullable: false),
                    MaxNotes = table.Column<int>(type: "integer", nullable: false),
                    MaxFolders = table.Column<int>(type: "integer", nullable: false),
                    MaxLabels = table.Column<int>(type: "integer", nullable: false),
                    MaxRelatedNotes = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentType",
                schema: "note_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileType",
                schema: "file",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FolderType",
                schema: "folder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FontSize",
                schema: "noots_systems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FontSize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HType",
                schema: "note_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "noots_systems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteTextType",
                schema: "note_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTextType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteType",
                schema: "note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefType",
                schema: "noots_systems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SortedByType",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortedByType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theme",
                schema: "noots_systems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppFile",
                schema: "file",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PathPhotoSmall = table.Column<string>(type: "text", nullable: true),
                    PathPhotoMedium = table.Column<string>(type: "text", nullable: true),
                    PathPhotoBig = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    PathNonPhotoContent = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    FileTypeId = table.Column<int>(type: "integer", nullable: false),
                    MetaData = table.Column<AppFileMetaData>(type: "jsonb", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppFile_FileType_FileTypeId",
                        column: x => x.FileTypeId,
                        principalSchema: "file",
                        principalTable: "FileType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppFileUploadInfo",
                schema: "file",
                columns: table => new
                {
                    AppFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UnLinkedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFileUploadInfo", x => x.AppFileId);
                    table.ForeignKey(
                        name: "FK_AppFileUploadInfo_AppFile_AppFileId",
                        column: x => x.AppFileId,
                        principalSchema: "file",
                        principalTable: "AppFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Background",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Background", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Background_AppFile_FileId",
                        column: x => x.FileId,
                        principalSchema: "file",
                        principalTable: "AppFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    DefaultPhotoUrl = table.Column<string>(type: "text", nullable: true),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    CurrentBackgroundId = table.Column<Guid>(type: "uuid", nullable: true),
                    ThemeId = table.Column<int>(type: "integer", nullable: false),
                    FontSizeId = table.Column<int>(type: "integer", nullable: false),
                    BillingPlanId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Background_CurrentBackgroundId",
                        column: x => x.CurrentBackgroundId,
                        principalSchema: "user",
                        principalTable: "Background",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_BillingPlan_BillingPlanId",
                        column: x => x.BillingPlanId,
                        principalSchema: "user",
                        principalTable: "BillingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_FontSize_FontSizeId",
                        column: x => x.FontSizeId,
                        principalSchema: "noots_systems",
                        principalTable: "FontSize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "noots_systems",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Theme_ThemeId",
                        column: x => x.ThemeId,
                        principalSchema: "noots_systems",
                        principalTable: "Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folder",
                schema: "folder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FolderTypeId = table.Column<int>(type: "integer", nullable: false),
                    RefTypeId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folder_FolderType_FolderTypeId",
                        column: x => x.FolderTypeId,
                        principalSchema: "folder",
                        principalTable: "FolderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_RefType_RefTypeId",
                        column: x => x.RefTypeId,
                        principalSchema: "noots_systems",
                        principalTable: "RefType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Label",
                schema: "label",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Label_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "note",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteTypeId = table.Column<int>(type: "integer", nullable: false),
                    RefTypeId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    UnlockTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_NoteType_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalSchema: "note",
                        principalTable: "NoteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Note_RefType_RefTypeId",
                        column: x => x.RefTypeId,
                        principalSchema: "noots_systems",
                        principalTable: "RefType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Note_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserFromId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserToId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSystemMessage = table.Column<bool>(type: "boolean", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    TranslateKeyMessage = table.Column<string>(type: "text", nullable: true),
                    AdditionalMessage = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserFromId",
                        column: x => x.UserFromId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserToId",
                        column: x => x.UserToId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSetting",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSetting_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalizationSetting",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortedNoteByTypeId = table.Column<int>(type: "integer", nullable: false),
                    SortedFolderByTypeId = table.Column<int>(type: "integer", nullable: false),
                    NotesInFolderCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    ContentInNoteCount = table.Column<int>(type: "integer", nullable: false),
                    IsViewVideoOnNote = table.Column<bool>(type: "boolean", nullable: false),
                    IsViewAudioOnNote = table.Column<bool>(type: "boolean", nullable: false),
                    IsViewPhotosOnNote = table.Column<bool>(type: "boolean", nullable: false),
                    IsViewTextOnNote = table.Column<bool>(type: "boolean", nullable: false),
                    IsViewDocumentOnNote = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalizationSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalizationSetting_SortedByType_SortedFolderByTypeId",
                        column: x => x.SortedFolderByTypeId,
                        principalSchema: "user",
                        principalTable: "SortedByType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalizationSetting_SortedByType_SortedNoteByTypeId",
                        column: x => x.SortedNoteByTypeId,
                        principalSchema: "user",
                        principalTable: "SortedByType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalizationSetting_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentifierConnectionId",
                schema: "ws",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConnectionId = table.Column<string>(type: "text", nullable: true),
                    ConnectedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentifierConnectionId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIdentifierConnectionId_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfilePhoto",
                schema: "user",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppFileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfilePhoto", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfilePhoto_AppFile_AppFileId",
                        column: x => x.AppFileId,
                        principalSchema: "file",
                        principalTable: "AppFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfilePhoto_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersOnPrivateFolders",
                schema: "folder",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FolderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersOnPrivateFolders", x => new { x.FolderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersOnPrivateFolders_Folder_FolderId",
                        column: x => x.FolderId,
                        principalSchema: "folder",
                        principalTable: "Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersOnPrivateFolders_RefType_AccessTypeId",
                        column: x => x.AccessTypeId,
                        principalSchema: "noots_systems",
                        principalTable: "RefType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersOnPrivateFolders_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseNoteContent",
                schema: "note_content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ContentTypeId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseNoteContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseNoteContent_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalSchema: "note_content",
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseNoteContent_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CacheNoteHistory",
                schema: "note_history",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersThatEditIds = table.Column<HashSet<Guid>>(type: "jsonb", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheNoteHistory", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_CacheNoteHistory_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoldersNotes",
                schema: "folder",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldersNotes", x => new { x.NoteId, x.FolderId });
                    table.ForeignKey(
                        name: "FK_FoldersNotes_Folder_FolderId",
                        column: x => x.FolderId,
                        principalSchema: "folder",
                        principalTable: "Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoldersNotes_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabelsNotes",
                schema: "label",
                columns: table => new
                {
                    LabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelsNotes", x => new { x.NoteId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_LabelsNotes_Label_LabelId",
                        column: x => x.LabelId,
                        principalSchema: "label",
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelsNotes_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteSnapshot",
                schema: "note_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteTypeId = table.Column<int>(type: "integer", nullable: false),
                    RefTypeId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Labels = table.Column<List<SnapshotNoteLabel>>(type: "jsonb", nullable: true),
                    Contents = table.Column<ContentSnapshot>(type: "jsonb", nullable: true),
                    SnapshotTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteSnapshot_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteSnapshot_NoteType_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalSchema: "note",
                        principalTable: "NoteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteSnapshot_RefType_RefTypeId",
                        column: x => x.RefTypeId,
                        principalSchema: "noots_systems",
                        principalTable: "RefType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedNoteToInnerNote",
                schema: "note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedNoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedNoteToInnerNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedNoteToInnerNote_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedNoteToInnerNote_Note_RelatedNoteId",
                        column: x => x.RelatedNoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOnPrivateNotes",
                schema: "note",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOnPrivateNotes", x => new { x.NoteId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserOnPrivateNotes_Note_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "note",
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOnPrivateNotes_RefType_AccessTypeId",
                        column: x => x.AccessTypeId,
                        principalSchema: "noots_systems",
                        principalTable: "RefType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOnPrivateNotes_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionNote",
                schema: "note_content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MetaData = table.Column<CollectionMetadata>(type: "jsonb", nullable: true),
                    FileTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionNote_BaseNoteContent_Id",
                        column: x => x.Id,
                        principalSchema: "note_content",
                        principalTable: "BaseNoteContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionNote_FileType_FileTypeId",
                        column: x => x.FileTypeId,
                        principalSchema: "file",
                        principalTable: "FileType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextNote",
                schema: "note_content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Contents = table.Column<List<TextBlock>>(type: "jsonb", nullable: true),
                    NoteTextTypeId = table.Column<int>(type: "integer", nullable: false),
                    HTypeId = table.Column<int>(type: "integer", nullable: true),
                    Checked = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextNote_BaseNoteContent_Id",
                        column: x => x.Id,
                        principalSchema: "note_content",
                        principalTable: "BaseNoteContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextNote_HType_HTypeId",
                        column: x => x.HTypeId,
                        principalSchema: "note_content",
                        principalTable: "HType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TextNote_NoteTextType_NoteTextTypeId",
                        column: x => x.NoteTextTypeId,
                        principalSchema: "note_content",
                        principalTable: "NoteTextType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotFileContent",
                schema: "note_history",
                columns: table => new
                {
                    NoteSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppFileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotFileContent", x => new { x.NoteSnapshotId, x.AppFileId });
                    table.ForeignKey(
                        name: "FK_SnapshotFileContent_AppFile_AppFileId",
                        column: x => x.AppFileId,
                        principalSchema: "file",
                        principalTable: "AppFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotFileContent_NoteSnapshot_NoteSnapshotId",
                        column: x => x.NoteSnapshotId,
                        principalSchema: "note_history",
                        principalTable: "NoteSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNoteSnapshotManyToMany",
                schema: "note_history",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteSnapshotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNoteSnapshotManyToMany", x => new { x.UserId, x.NoteSnapshotId });
                    table.ForeignKey(
                        name: "FK_UserNoteSnapshotManyToMany_NoteSnapshot_NoteSnapshotId",
                        column: x => x.NoteSnapshotId,
                        principalSchema: "note_history",
                        principalTable: "NoteSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNoteSnapshotManyToMany_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedNoteUserState",
                schema: "note",
                columns: table => new
                {
                    ReletatedNoteInnerNoteId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedNoteInnerNoteId = table.Column<int>(type: "integer", nullable: true),
                    IsOpened = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedNoteUserState", x => new { x.UserId, x.ReletatedNoteInnerNoteId });
                    table.ForeignKey(
                        name: "FK_RelatedNoteUserState_RelatedNoteToInnerNote_RelatedNoteInne~",
                        column: x => x.RelatedNoteInnerNoteId,
                        principalSchema: "note",
                        principalTable: "RelatedNoteToInnerNote",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RelatedNoteUserState_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionNoteAppFile",
                schema: "note_content",
                columns: table => new
                {
                    AppFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionNoteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionNoteAppFile", x => new { x.CollectionNoteId, x.AppFileId });
                    table.ForeignKey(
                        name: "FK_CollectionNoteAppFile_AppFile_AppFileId",
                        column: x => x.AppFileId,
                        principalSchema: "file",
                        principalTable: "AppFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionNoteAppFile_CollectionNote_CollectionNoteId",
                        column: x => x.CollectionNoteId,
                        principalSchema: "note_content",
                        principalTable: "CollectionNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "user",
                table: "BillingPlan",
                columns: new[] { "Id", "MaxFolders", "MaxLabels", "MaxNotes", "MaxRelatedNotes", "MaxSize", "Name" },
                values: new object[,]
                {
                    { 1, 250, 500, 250, 5, 1048576000L, "Free" },
                    { 2, 10000, 10000, 10000, 30, 5242880000L, "Standart" }
                });

            migrationBuilder.InsertData(
                schema: "note_content",
                table: "ContentType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Text" },
                    { 2, "Collection" }
                });

            migrationBuilder.InsertData(
                schema: "file",
                table: "FileType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Audio" },
                    { 2, "Photo" },
                    { 3, "Video" },
                    { 4, "Document" }
                });

            migrationBuilder.InsertData(
                schema: "folder",
                table: "FolderType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Private" },
                    { 2, "Shared" },
                    { 3, "Archived" },
                    { 4, "Deleted" }
                });

            migrationBuilder.InsertData(
                schema: "noots_systems",
                table: "FontSize",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Big" },
                    { 2, "Medium" }
                });

            migrationBuilder.InsertData(
                schema: "note_content",
                table: "HType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "H1" },
                    { 2, "H2" },
                    { 3, "H3" }
                });

            migrationBuilder.InsertData(
                schema: "noots_systems",
                table: "Language",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "Ukraine" },
                    { 3, "Russian" },
                    { 4, "Spanish" },
                    { 5, "French" },
                    { 6, "Italian" },
                    { 7, "German" },
                    { 8, "Swedish" },
                    { 9, "Polish" },
                    { 10, "Chinese" },
                    { 11, "Japan" }
                });

            migrationBuilder.InsertData(
                schema: "note_content",
                table: "NoteTextType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Default" },
                    { 2, "Heading" },
                    { 3, "Dotlist" },
                    { 4, "Numberlist" },
                    { 5, "Checklist" }
                });

            migrationBuilder.InsertData(
                schema: "note",
                table: "NoteType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Private" },
                    { 2, "Shared" },
                    { 3, "Archived" },
                    { 4, "Deleted" }
                });

            migrationBuilder.InsertData(
                schema: "noots_systems",
                table: "RefType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Viewer" },
                    { 2, "Editor" }
                });

            migrationBuilder.InsertData(
                schema: "user",
                table: "SortedByType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "AscDate" },
                    { 2, "DescDate" },
                    { 3, "CustomOrder" }
                });

            migrationBuilder.InsertData(
                schema: "noots_systems",
                table: "Theme",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Dark" },
                    { 2, "Light" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppFile_FileTypeId",
                schema: "file",
                table: "AppFile",
                column: "FileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFile_UserId",
                schema: "file",
                table: "AppFile",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Background_FileId",
                schema: "user",
                table: "Background",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Background_UserId",
                schema: "user",
                table: "Background",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNoteContent_ContentTypeId",
                schema: "note_content",
                table: "BaseNoteContent",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNoteContent_NoteId",
                schema: "note_content",
                table: "BaseNoteContent",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionNote_FileTypeId",
                schema: "note_content",
                table: "CollectionNote",
                column: "FileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionNoteAppFile_AppFileId",
                schema: "note_content",
                table: "CollectionNoteAppFile",
                column: "AppFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_FolderTypeId",
                schema: "folder",
                table: "Folder",
                column: "FolderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_RefTypeId",
                schema: "folder",
                table: "Folder",
                column: "RefTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_UserId",
                schema: "folder",
                table: "Folder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FoldersNotes_FolderId",
                schema: "folder",
                table: "FoldersNotes",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_UserId",
                schema: "label",
                table: "Label",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelsNotes_LabelId",
                schema: "label",
                table: "LabelsNotes",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_NoteTypeId",
                schema: "note",
                table: "Note",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_RefTypeId",
                schema: "note",
                table: "Note",
                column: "RefTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_UserId",
                schema: "note",
                table: "Note",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteSnapshot_NoteId",
                schema: "note_history",
                table: "NoteSnapshot",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteSnapshot_NoteTypeId",
                schema: "note_history",
                table: "NoteSnapshot",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteSnapshot_RefTypeId",
                schema: "note_history",
                table: "NoteSnapshot",
                column: "RefTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserFromId",
                schema: "user",
                table: "Notification",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserToId",
                schema: "user",
                table: "Notification",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSetting_UserId",
                schema: "user",
                table: "NotificationSetting",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizationSetting_SortedFolderByTypeId",
                schema: "user",
                table: "PersonalizationSetting",
                column: "SortedFolderByTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizationSetting_SortedNoteByTypeId",
                schema: "user",
                table: "PersonalizationSetting",
                column: "SortedNoteByTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizationSetting_UserId",
                schema: "user",
                table: "PersonalizationSetting",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatedNoteToInnerNote_NoteId",
                schema: "note",
                table: "RelatedNoteToInnerNote",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedNoteToInnerNote_RelatedNoteId",
                schema: "note",
                table: "RelatedNoteToInnerNote",
                column: "RelatedNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedNoteUserState_RelatedNoteInnerNoteId",
                schema: "note",
                table: "RelatedNoteUserState",
                column: "RelatedNoteInnerNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotFileContent_AppFileId",
                schema: "note_history",
                table: "SnapshotFileContent",
                column: "AppFileId");

            migrationBuilder.CreateIndex(
                name: "IX_TextNote_HTypeId",
                schema: "note_content",
                table: "TextNote",
                column: "HTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TextNote_NoteTextTypeId",
                schema: "note_content",
                table: "TextNote",
                column: "NoteTextTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_BillingPlanId",
                schema: "user",
                table: "User",
                column: "BillingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CurrentBackgroundId",
                schema: "user",
                table: "User",
                column: "CurrentBackgroundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "user",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_FontSizeId",
                schema: "user",
                table: "User",
                column: "FontSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_LanguageId",
                schema: "user",
                table: "User",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ThemeId",
                schema: "user",
                table: "User",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifierConnectionId_UserId",
                schema: "ws",
                table: "UserIdentifierConnectionId",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNoteSnapshotManyToMany_NoteSnapshotId",
                schema: "note_history",
                table: "UserNoteSnapshotManyToMany",
                column: "NoteSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOnPrivateNotes_AccessTypeId",
                schema: "note",
                table: "UserOnPrivateNotes",
                column: "AccessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOnPrivateNotes_UserId",
                schema: "note",
                table: "UserOnPrivateNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfilePhoto_AppFileId",
                schema: "user",
                table: "UserProfilePhoto",
                column: "AppFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersOnPrivateFolders_AccessTypeId",
                schema: "folder",
                table: "UsersOnPrivateFolders",
                column: "AccessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersOnPrivateFolders_UserId",
                schema: "folder",
                table: "UsersOnPrivateFolders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFile_User_UserId",
                schema: "file",
                table: "AppFile",
                column: "UserId",
                principalSchema: "user",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Background_User_UserId",
                schema: "user",
                table: "Background",
                column: "UserId",
                principalSchema: "user",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFile_FileType_FileTypeId",
                schema: "file",
                table: "AppFile");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFile_User_UserId",
                schema: "file",
                table: "AppFile");

            migrationBuilder.DropForeignKey(
                name: "FK_Background_User_UserId",
                schema: "user",
                table: "Background");

            migrationBuilder.DropTable(
                name: "AppFileUploadInfo",
                schema: "file");

            migrationBuilder.DropTable(
                name: "CacheNoteHistory",
                schema: "note_history");

            migrationBuilder.DropTable(
                name: "CollectionNoteAppFile",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "FoldersNotes",
                schema: "folder");

            migrationBuilder.DropTable(
                name: "LabelsNotes",
                schema: "label");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "user");

            migrationBuilder.DropTable(
                name: "NotificationSetting",
                schema: "user");

            migrationBuilder.DropTable(
                name: "PersonalizationSetting",
                schema: "user");

            migrationBuilder.DropTable(
                name: "RelatedNoteUserState",
                schema: "note");

            migrationBuilder.DropTable(
                name: "SnapshotFileContent",
                schema: "note_history");

            migrationBuilder.DropTable(
                name: "TextNote",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "UserIdentifierConnectionId",
                schema: "ws");

            migrationBuilder.DropTable(
                name: "UserNoteSnapshotManyToMany",
                schema: "note_history");

            migrationBuilder.DropTable(
                name: "UserOnPrivateNotes",
                schema: "note");

            migrationBuilder.DropTable(
                name: "UserProfilePhoto",
                schema: "user");

            migrationBuilder.DropTable(
                name: "UsersOnPrivateFolders",
                schema: "folder");

            migrationBuilder.DropTable(
                name: "CollectionNote",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "Label",
                schema: "label");

            migrationBuilder.DropTable(
                name: "SortedByType",
                schema: "user");

            migrationBuilder.DropTable(
                name: "RelatedNoteToInnerNote",
                schema: "note");

            migrationBuilder.DropTable(
                name: "HType",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "NoteTextType",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "NoteSnapshot",
                schema: "note_history");

            migrationBuilder.DropTable(
                name: "Folder",
                schema: "folder");

            migrationBuilder.DropTable(
                name: "BaseNoteContent",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "FolderType",
                schema: "folder");

            migrationBuilder.DropTable(
                name: "ContentType",
                schema: "note_content");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "note");

            migrationBuilder.DropTable(
                name: "NoteType",
                schema: "note");

            migrationBuilder.DropTable(
                name: "RefType",
                schema: "noots_systems");

            migrationBuilder.DropTable(
                name: "FileType",
                schema: "file");

            migrationBuilder.DropTable(
                name: "User",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Background",
                schema: "user");

            migrationBuilder.DropTable(
                name: "BillingPlan",
                schema: "user");

            migrationBuilder.DropTable(
                name: "FontSize",
                schema: "noots_systems");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "noots_systems");

            migrationBuilder.DropTable(
                name: "Theme",
                schema: "noots_systems");

            migrationBuilder.DropTable(
                name: "AppFile",
                schema: "file");
        }
    }
}