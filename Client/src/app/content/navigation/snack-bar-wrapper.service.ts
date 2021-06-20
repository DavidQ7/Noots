/* eslint-disable class-methods-use-this */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { take } from 'rxjs/operators';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { LanguagesENUM } from 'src/app/shared/enums/LanguagesENUM';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { BaseChangeTypeSmallFolder } from '../folders/state/folders-actions';
import { BaseChangeTypeSmallNote } from '../notes/state/notes-actions';

@Injectable({
  providedIn: 'root',
})
export class SnackBarWrapperService {
  constructor(private snackService: SnackbarService, private store: Store) {}

  // eslint-disable-next-line class-methods-use-this
  getNotesNaming(isMany: boolean, language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return isMany ? 'Notes' : 'Note';
      }
      case LanguagesENUM.Russian: {
        return isMany ? 'Заметки' : 'Заметка';
      }
      case LanguagesENUM.Ukraine: {
        return isMany ? 'Нотатки' : 'Нотаток';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  // eslint-disable-next-line class-methods-use-this
  getFoldersNaming(isMany: boolean, language: LanguagesENUM): string {
    switch (language) {
      case LanguagesENUM.English: {
        return isMany ? 'Folders' : 'Folder';
      }
      case LanguagesENUM.Russian: {
        return isMany ? 'Папки' : 'Папка';
      }
      case LanguagesENUM.Ukraine: {
        return isMany ? 'Папки' : 'Папка';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  // eslint-disable-next-line class-methods-use-this
  getMoveToMessage(isMany: boolean, language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return isMany ? 'moved to' : 'moved to';
      }
      case LanguagesENUM.Russian: {
        return isMany ? 'перенесены в' : 'перенесена в';
      }
      case LanguagesENUM.Ukraine: {
        return isMany ? 'перенесені в' : 'перенесена в';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  // eslint-disable-next-line class-methods-use-this
  getUndoMessage(language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return 'Undo';
      }
      case LanguagesENUM.Russian: {
        return 'Отменить';
      }
      case LanguagesENUM.Ukraine: {
        return 'Відмінити';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  getArchiveEntityName(isMany: boolean, language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return 'archive';
      }
      case LanguagesENUM.Russian: {
        return 'архив';
      }
      case LanguagesENUM.Ukraine: {
        return 'архів';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  getPrivateEntityName(isMany: boolean, language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return 'private';
      }
      case LanguagesENUM.Russian: {
        return 'личные';
      }
      case LanguagesENUM.Ukraine: {
        return 'особисті';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  getDeleteEntityName(isMany: boolean, language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return 'bin';
      }
      case LanguagesENUM.Russian: {
        return 'корзину';
      }
      case LanguagesENUM.Ukraine: {
        return 'кошик';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  getSharedEntityName(isMany: boolean, language: LanguagesENUM) {
    switch (language) {
      case LanguagesENUM.English: {
        return 'shared';
      }
      case LanguagesENUM.Russian: {
        return 'общие';
      }
      case LanguagesENUM.Ukraine: {
        return 'спільні';
      }
      default: {
        throw new Error('error');
      }
    }
  }

  buildArchive(
    callback: BaseChangeTypeSmallNote | BaseChangeTypeSmallFolder,
    entityNameOp: (isMany: boolean, language: LanguagesENUM) => string,
  ) {
    const lname = this.store.selectSnapshot(UserStore.getUserLanguage);
    const message = this.concatMessages(
      lname,
      callback.selectedIds.length > 1,
      entityNameOp,
      this.getMoveToMessage,
      this.getArchiveEntityName,
    );
    this.buildNotification(message, this.getUndoMessage(lname), callback);
  }

  buildPrivate(
    callback: BaseChangeTypeSmallNote | BaseChangeTypeSmallFolder,
    entityNameOp: (isMany: boolean, language: LanguagesENUM) => string,
  ) {
    const lname = this.store.selectSnapshot(UserStore.getUserLanguage);
    const message = this.concatMessages(
      lname,
      callback.selectedIds.length > 1,
      entityNameOp,
      this.getMoveToMessage,
      this.getArchiveEntityName,
    );
    this.buildNotification(message, this.getUndoMessage(lname), callback);
  }

  buildDelete(
    callback: BaseChangeTypeSmallNote | BaseChangeTypeSmallFolder,
    entityNameOp: (isMany: boolean, language: LanguagesENUM) => string,
  ) {
    const lname = this.store.selectSnapshot(UserStore.getUserLanguage);
    const message = this.concatMessages(
      lname,
      callback.selectedIds.length > 1,
      entityNameOp,
      this.getMoveToMessage,
      this.getDeleteEntityName,
    );
    this.buildNotification(message, this.getUndoMessage(lname), callback);
  }

  buildNotification(message: string, undoMessage?: string, callbackAction?: any) {
    const snackbarRef = this.buildSnackBarRef(message, undoMessage);

    if (callbackAction) {
      snackbarRef
        .afterDismissed()
        .pipe(take(1))
        .subscribe((x) => {
          if (x.dismissedByAction) {
            this.store.dispatch(callbackAction);
          }
        });
    }
    return snackbarRef;
  }

  concatMessages(
    language: LanguagesENUM,
    isMany: boolean,
    ...ops: ((isMany: boolean, language: LanguagesENUM) => string)[]
  ): string {
    let result = '';
    // eslint-disable-next-line no-return-assign
    ops.forEach((func) => {
      result += func(isMany, language);
      result += ' ';
    });
    return result;
  }

  buildSnackBarRef(message: string, undo?: string) {
    return this.snackService.openSnackBar(message, undo);
  }
}