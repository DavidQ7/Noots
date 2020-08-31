import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Folder } from './models/folder';
import { environment } from 'src/environments/environment';
import { FullFolder } from './models/FullFolder';
import { FolderType } from 'src/app/shared/enums/FolderTypes';

@Injectable()
export class ApiFoldersService {

  constructor(private httpClient: HttpClient) { }

  getPrivateFolders() {
    return this.httpClient.get<Folder[]>(environment.writeAPI + '/api/folder/private');
  }

  getSharedFolders() {
    return this.httpClient.get<Folder[]>(environment.writeAPI + '/api/folder/shared');
  }

  getDeletedFolders() {
    return this.httpClient.get<Folder[]>(environment.writeAPI + '/api/folder/deleted');
  }

  getArchiveFolders() {
    return this.httpClient.get<Folder[]>(environment.writeAPI + '/api/folder/archive');
  }

  get(id: string) {
    return this.httpClient.get<FullFolder>(environment.writeAPI + `/api/folder/${id}`);
  }

  new() {
    return this.httpClient.get<string>(environment.writeAPI + `/api/folder/new`);
  }

  archiveFolder(ids: string[], folderType: FolderType) {
    const obj = {
      ids,
      folderType
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/archive`, obj);
  }

  setDeleteFolder(ids: string[], folderType: FolderType) {
    const obj = {
      ids,
      folderType
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/delete`, obj);
  }

  deleteFolders(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/delete/permanently`, obj);
  }

  copyFolders(ids: string[], folderType: FolderType) {
    const obj = {
      ids,
      folderType
    };
    return this.httpClient.patch<Folder[]>(environment.writeAPI + `/api/folder/copy`, obj);
  }

  restoreFolder(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/restore`, obj);
  }

  changeColor(ids: string[], color: string) {
    const obj = {
      ids,
      color
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/color`, obj);
  }

  makePublicFolders(ids: string[], folderType: FolderType) {
    const obj = {
      ids,
      folderType
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/ref/public`, obj);
  }

  makePrivateFolders(ids: string[], folderType: FolderType) {
    const obj = {
      ids,
      folderType
    };
    return this.httpClient.patch(environment.writeAPI + `/api/folder/ref/private`, obj);
  }


}