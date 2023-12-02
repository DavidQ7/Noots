import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as JSZip from 'jszip';
import { forkJoin, Observable } from 'rxjs';
import { finalize, map, takeUntil } from 'rxjs/operators';
import { saveAs } from 'file-saver';
import {
  FileProcessTracker,
  SnackBarFileProcessHandlerService,
} from 'src/app/shared/services/snackbar/snack-bar-file-process-handler.service';
import { LongTermOperationsHandlerService } from '../long-term-operations-handler/services/long-term-operations-handler.service';
import { LongTermsIcons } from '../long-term-operations-handler/models/long-terms.icons';
import {
  LongTermOperation,
  OperationDetailMini,
} from '../long-term-operations-handler/models/long-term-operation';
import { SnackbarService } from 'src/app/shared/services/snackbar/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import dayjs from 'dayjs';
import { InterceptorSkipToken } from 'src/app/core/token-interceptor.service';
import { AudiosCollection, AudioModel } from 'src/app/editor/entities/contents/audios-collection';
import { DocumentsCollection, DocumentModel } from 'src/app/editor/entities/contents/documents-collection';
import { PhotosCollection, Photo } from 'src/app/editor/entities/contents/photos-collection';
import { VideosCollection, VideoModel } from 'src/app/editor/entities/contents/videos-collection';

@Injectable({
  providedIn: 'root',
})
export class ExportService {
  constructor(
    private httpClient: HttpClient,
    private longTermOperationsHandler: LongTermOperationsHandlerService,
    private snackBarFileProcessingHandler: SnackBarFileProcessHandlerService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService,
  ) {}

  zipFiles = async (tasks: Observable<{ blob: FileProcessTracker<Blob>; name: string }>[]) => {
    const resp = await forkJoin(tasks).toPromise();
    const zip = new JSZip();
    resp.forEach((x) => zip.file(x.name, x.blob.eventBody));
    const zipFile = await zip.generateAsync({ type: 'blob' });
    saveAs(zipFile, `x-notes-export ${dayjs().format('LLL')}`);
  };

  getBlobFile(url: string, mini: OperationDetailMini, operation: LongTermOperation) {
    const headers = new HttpHeaders().set(InterceptorSkipToken, '');
    return this.httpClient
      .get(url, {
        headers,
        responseType: 'blob',
        reportProgress: true,
        observe: 'events',
      })
      .pipe(
        finalize(() => this.longTermOperationsHandler.finalize(operation, mini)),
        takeUntil(mini.obs),
        (x) => this.snackBarFileProcessingHandler.trackProcess(x, mini),
      );
  }

  // PHOTOS
  async exportAlbum(collection: PhotosCollection) {
    if (collection.items.length === 0) {
      const message = this.translateService.instant('snackBar.noItemInExport');
      this.snackbarService.openSnackBar(message, null, null);
      return;
    }
    const operation = this.longTermOperationsHandler.addNewExportOperation('uploader.exportPhotos');
    const tasks = collection.items.map((photo) => {
      const path = photo.photoFromBig;
      const mini = this.longTermOperationsHandler.getNewMini(
        operation,
        LongTermsIcons.Image,
        photo.name,
        false,
      );
      return this.getBlobFile(path, mini, operation).pipe(
        map((blob) => {
          return {
            blob,
            name: photo.name,
          };
        }),
      );
    });

    const miniFinal = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Export,
      'uploader.exportShort',
      false,
      false,
      true,
    );
    await this.zipFiles(tasks);
    this.longTermOperationsHandler.finalize(operation, miniFinal);
  }

  async exportPhoto(photo: Photo) {
    const operation = this.longTermOperationsHandler.addNewExportOperation('uploader.exportPhotos');
    const mini = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Image,
      photo.name,
      false,
    );
    const path = photo.photoFromBig;
    const blob = await this.getBlobFile(path, mini, operation).toPromise();
    saveAs(blob.eventBody, photo.name);
  }

  // AUDIOS
  async exportPlaylist(collection: AudiosCollection) {
    if (collection.items.length === 0) {
      const message = this.translateService.instant('snackBar.noItemInExport');
      this.snackbarService.openSnackBar(message, null, null);
      return;
    }
    const operation = this.longTermOperationsHandler.addNewExportOperation('uploader.exportAudios');
    const tasks = collection.items.map((audio) => {
      const mini = this.longTermOperationsHandler.getNewMini(
        operation,
        LongTermsIcons.Audio,
        audio.name,
        false,
      );
      const path = audio.audioPath;
      return this.getBlobFile(path, mini, operation).pipe(
        map((blob) => {
          return {
            blob,
            name: audio.name,
          };
        }),
      );
    });

    const miniFinal = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Export,
      'uploader.exportShort',
      false,
      false,
      true,
    );
    await this.zipFiles(tasks);
    this.longTermOperationsHandler.finalize(operation, miniFinal);
  }

  async exportAudio(audio: AudioModel) {
    const operation = this.longTermOperationsHandler.addNewExportOperation('uploader.exportAudios');
    const mini = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Audio,
      audio.name,
      false,
    );
    const path = audio.audioPath;
    const blob = await this.getBlobFile(path, mini, operation).toPromise();
    saveAs(blob.eventBody, audio.name);
  }

  // DOCUMENT
  async exportDocuments(collection: DocumentsCollection) {
    if (collection.items.length === 0) {
      const message = this.translateService.instant('snackBar.noItemInExport');
      this.snackbarService.openSnackBar(message, null, null);
      return;
    }
    const operation = this.longTermOperationsHandler.addNewExportOperation(
      'uploader.exportDocuments',
    );
    const tasks = collection.items.map((document) => {
      const mini = this.longTermOperationsHandler.getNewMini(
        operation,
        LongTermsIcons.Document,
        document.name,
        false,
      );
      const path = document.documentPath;
      return this.getBlobFile(path, mini, operation).pipe(
        map((blob) => {
          return {
            blob,
            name: document.name,
          };
        }),
      );
    });

    const miniFinal = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Export,
      'uploader.exportShort',
      false,
      false,
      true,
    );
    await this.zipFiles(tasks);
    this.longTermOperationsHandler.finalize(operation, miniFinal);
  }

  async exportDocument(document: DocumentModel) {
    const operation = this.longTermOperationsHandler.addNewExportOperation(
      'uploader.exportDocuments',
    );
    const mini = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Document,
      document.name,
      false,
    );
    const path = document.documentPath;
    const blob = await this.getBlobFile(path, mini, operation).toPromise();
    saveAs(blob.eventBody, document.name);
  }

  // VIDEOS
  async exportVideos(collection: VideosCollection) {
    if (collection.items.length === 0) {
      const message = this.translateService.instant('snackBar.noItemInExport');
      this.snackbarService.openSnackBar(message, null, null);
      return;
    }
    const operation = this.longTermOperationsHandler.addNewExportOperation('uploader.exportVideos');
    const tasks = collection.items.map((video) => {
      const mini = this.longTermOperationsHandler.getNewMini(
        operation,
        LongTermsIcons.Video,
        video.name,
        false,
      );
      const path = video.videoPath;
      return this.getBlobFile(path, mini, operation).pipe(
        map((blob) => {
          return {
            blob,
            name: video.name,
          };
        }),
      );
    });

    const miniFinal = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Export,
      'uploader.exportShort',
      false,
      false,
      true,
    );
    await this.zipFiles(tasks);
    this.longTermOperationsHandler.finalize(operation, miniFinal);
  }

  async exportVideo(video: VideoModel) {
    const operation = this.longTermOperationsHandler.addNewExportOperation('uploader.exportVideos');
    const mini = this.longTermOperationsHandler.getNewMini(
      operation,
      LongTermsIcons.Video,
      video.name,
      false,
    );
    const path = video.videoPath;
    const blob = await this.getBlobFile(path, mini, operation).toPromise();
    saveAs(blob.eventBody, video.name);
  }
}