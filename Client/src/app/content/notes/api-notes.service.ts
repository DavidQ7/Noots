import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { NoteTypeENUM } from 'src/app/shared/enums/note-types.enum';
import { Observable } from 'rxjs';
import { RefTypeENUM } from 'src/app/shared/enums/ref-type.enum';
import { PersonalizationSetting } from 'src/app/core/models/personalization-setting.model';
import { TransformNoteUtil } from 'src/app/shared/services/transform-note.util';
import { SmallNote } from './models/small-note.model';
import { RequestFullNote } from './models/request-full-note.model';
import { Notes } from './state/notes.model';
import { InvitedUsersToNoteOrFolder } from './models/invited-users-to-note.model';
import {
  ContentModel,
} from './models/content-model.model';
import { OnlineUsersNote } from './models/online-users-note.model';
import { BottomNoteContent } from './models/bottom-note-content.model';

@Injectable()
export class ApiServiceNotes {
  constructor(private httpClient: HttpClient) {}

  getNotes(type: NoteTypeENUM, settings: PersonalizationSetting) {
    let params = new HttpParams();
    if (settings) {
      Object.keys(settings).forEach((key) => {
        params = params.append(key, settings[key]);
      });
    }
    return this.httpClient
      .get<SmallNote[]>(`${environment.writeAPI}/api/note/type/${type}`, { params })
      .pipe(
        map((z) => TransformNoteUtil.transformNotes(z)),
        map((notes) => new Notes(type, notes)),
      );
  }

  getNotesMany(noteIds: string[], settings: PersonalizationSetting) {
    const obj = {
      noteIds,
      settings,
    };

    return this.httpClient
      .post<SmallNote[]>(`${environment.writeAPI}/api/note/many`, obj)
      .pipe(map((z) => TransformNoteUtil.transformNotes(z)));
  }

  getAdditionalInfos(noteIds: string[]) {
    const obj = {
      noteIds,
    };
    return this.httpClient.post<BottomNoteContent[]>(
      `${environment.writeAPI}/api/note/additional`,
      obj,
    );
  }

  addLabel(labelId: string, noteIds: string[]) {
    const obj = {
      labelId,
      noteIds,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/label/add`, obj);
  }

  removeLabel(labelId: string, noteIds: string[]) {
    const obj = {
      labelId,
      noteIds,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/label/remove`, obj);
  }

  changeColor(ids: string[], color: string) {
    const obj = {
      ids,
      color,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/color`, obj);
  }

  setDeleteNotes(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/delete`, obj);
  }

  makePrivateNotes(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/ref/private`, obj);
  }

  copyNotes(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch<string[]>(`${environment.writeAPI}/api/note/copy`, obj);
  }

  deleteNotes(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/delete/permanently`, obj);
  }

  archiveNotes(ids: string[]) {
    const obj = {
      ids,
    };
    return this.httpClient.patch(`${environment.writeAPI}/api/note/archive`, obj);
  }

  get(id: string) {
    return this.httpClient.get<RequestFullNote>(`${environment.writeAPI}/api/note/${id}`);
  }

  getAll(settings: PersonalizationSetting) {
    const obj = {
      settings,
    };
    return this.httpClient
      .post<SmallNote[]>(`${environment.writeAPI}/api/note/all`, obj)
      .pipe(map((z) => TransformNoteUtil.transformNotes(z)));
  }

  new() {
    return this.httpClient.get<SmallNote>(`${environment.writeAPI}/api/note/new`);
  }

  getUsersOnPrivateNote(id: string) {
    return this.httpClient.get<InvitedUsersToNoteOrFolder[]>(
      `${environment.writeAPI}/api/share/notes/user/invites/${id}`,
    );
  }

  getOnlineUsersOnNote(id: string) {
    return this.httpClient.get<OnlineUsersNote[]>(
      `${environment.writeAPI}/api/fullnote/users/${id}`,
    );
  }

  makePublic(refTypeId: RefTypeENUM, ids: string[]) {
    const obj = {
      refTypeId,
      ids,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/share/notes/share`, obj);
  }

  sendInvitesToNote(
    userIds: string[],
    noteId: string,
    refTypeId: RefTypeENUM,
    sendMessage: boolean,
    message: string,
  ) {
    const obj = {
      userIds,
      noteId,
      refTypeId,
      sendMessage,
      message,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/share/notes/user/invites`, obj);
  }

  removeUserFromPrivateNote(noteId: string, userId: string) {
    const obj = {
      noteId,
      userId,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/share/notes/user/remove`, obj);
  }

  changeUserPermission(noteId: string, userId: string, accessTypeId: RefTypeENUM) {
    const obj = {
      noteId,
      userId,
      accessTypeId,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/share/notes/user/permission`, obj);
  }

  // CONTENTS

  getContents(noteId: string): Observable<ContentModel[]> {
    return this.httpClient
      .get<ContentModel[]>(`${environment.writeAPI}/api/fullnote/contents/${noteId}`)
      .pipe(map((x) => TransformNoteUtil.transformContent(x)));
  }

  // LINKS

  getMetaLink(url: string) {
    const obj = {
      url,
    };
    return this.httpClient.post<any>(`${environment.writeAPI}/api/AvoidProxy`, obj);
  }
}
