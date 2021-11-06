import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { forkJoin } from 'rxjs';
import { LongTermOperationsHandlerService } from 'src/app/content/long-term-operations-handler/services/long-term-operations-handler.service';
import { generateFormData, nameForUploadAudios } from 'src/app/core/defaults/form-data-generator';
import { LoadUsedDiskSpace } from 'src/app/core/stateUser/user-action';
import { SnackBarFileProcessHandlerService } from 'src/app/shared/services/snackbar/snack-bar-file-process-handler.service';
import { SnackBarHandlerStatusService } from 'src/app/shared/services/snackbar/snack-bar-handler-status.service';
import { UploadFilesService } from 'src/app/shared/services/upload-files.service';
import { finalize, takeUntil } from 'rxjs/operators';
import { LongTermsIcons } from 'src/app/content/long-term-operations-handler/models/long-terms.icons';
import { AudiosCollection } from '../../../models/content-model.model';
import { UploadFileToEntity } from '../../models/upload-files-to-entity';
import { ApiPlaylistService } from '../../services/api-playlist.service';
import { ContentEditorFilesBase } from './content-editor-files-base';
import { ContentEditorContentsService } from '../content-editor-contents.service';
import { OperationResult } from 'src/app/shared/models/operation-result.model';

@Injectable()
export class ContentEditorAudiosCollectionService extends ContentEditorFilesBase {
  constructor(
    store: Store,
    snackBarStatusTranslateService: SnackBarHandlerStatusService,
    uploadFilesService: UploadFilesService,
    longTermOperationsHandler: LongTermOperationsHandlerService,
    snackBarFileProcessingHandler: SnackBarFileProcessHandlerService,
    private apiAudiosCollection: ApiPlaylistService,
    contentEditorContentsService: ContentEditorContentsService,
  ) {
    super(
      store,
      snackBarStatusTranslateService,
      uploadFilesService,
      longTermOperationsHandler,
      snackBarFileProcessingHandler,
      contentEditorContentsService,
    );
  }

  async transformToAudiosCollection(noteId: string, contentId: string, files: File[]) {
    const collectionResult = await this.apiAudiosCollection.transformToPlaylist(noteId, contentId).toPromise();
    if (collectionResult.success) {
      collectionResult.data.isLoading = true; // TODO TRY CATCH
      collectionResult.data.audios = collectionResult.data.audios ? collectionResult.data.audios : [];
      this.transformContentToOrWarning(collectionResult, contentId);
      await this.uploadAudiosToCollectionHandler({ contentId: collectionResult.data.id, files }, noteId);
      collectionResult.data.isLoading = false;
    }
  }

  uploadAudiosToCollectionHandler = async ($event: UploadFileToEntity, noteId: string) => {
    const isCan = await this.uploadFilesService.isCanUserUploadFiles($event.files);
    if (!isCan) {
      return;
    }

    const operation = this.longTermOperationsHandler.addNewUploadToNoteOperation(
      'uploader.uploadingAudiosNoteLong',
      'uploader.uploading',
      'uploader.uploadingAudiosNote',
    );

    const uploadsRequests = $event.files.map((file) => {
      const formData = generateFormData([file], nameForUploadAudios);
      const mini = this.longTermOperationsHandler.getNewMini(
        operation,
        LongTermsIcons.Audio,
        file.name,
      );
      return this.apiAudiosCollection
        .uploadAudiosToPlaylist(formData, noteId, $event.contentId)
        .pipe(
          finalize(() => this.longTermOperationsHandler.finalize(operation, mini)),
          takeUntil(mini.obs),
          (x) => this.snackBarFileProcessingHandler.trackProcess(x, mini),
        );
    });

    const results = await forkJoin(uploadsRequests).toPromise();
    const audios = results
      .map((x) => x.eventBody)
      .filter((x) => x?.success)
      .map((x) => x?.data)
      .flat();

    if (!audios || audios.length === 0) {
      return;
    }

    const prevCollection = this.contentsService.getContentById<AudiosCollection>($event.contentId);
    const prev = prevCollection.audios ?? [];

    const newCollection = new AudiosCollection(prevCollection);
    newCollection.audios = [...prev, ...audios];

    this.contentsService.setSafeContentsAndSyncContents(newCollection, $event.contentId);

    this.afterUploadFilesToCollection(results);
  };

  deleteContentHandler = async (contentId: string, noteId: string): Promise<OperationResult<any>> => {
    const resp = await this.apiAudiosCollection.removePlaylist(noteId, contentId).toPromise();
    if (resp.success) {
      this.deleteHandler(contentId);
    }
    return resp;
  };

  async deleteAudioHandler(audioId: string, contentId: string, noteId: string) {
    const resp = await this.apiAudiosCollection
      .removeAudioFromPlaylist(noteId, contentId, audioId)
      .toPromise();

    if (resp.success) {
      const prevCollection = this.contentsService.getContentById<AudiosCollection>(contentId);
      if (prevCollection.audios.length === 1) {
        this.deleteHandler(contentId);
      } else {
        const newCollection = prevCollection.copy();
        newCollection.audios = newCollection.audios.filter((x) => x.fileId !== audioId);
        this.contentsService.setSafe(newCollection, contentId);
      }
      this.store.dispatch(LoadUsedDiskSpace);
    }
  }

  async changePlaylistName(contentId: string, noteId: string, name: string) {
    const resp = await this.apiAudiosCollection
      .changePlaylistName(noteId, contentId, name)
      .toPromise();
    if (resp.success) {
      const collection = this.contentsService.getContentById<AudiosCollection>(contentId);
      collection.name = name;
    }
  }
}
