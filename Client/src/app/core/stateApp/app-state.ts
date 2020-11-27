import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { UpdateRoute, SpinnerChangeStatus, SetToken, TokenSetNoUpdate } from './app-action';
import { EntityType } from 'src/app/shared/enums/EntityTypes';
import { NoteType } from 'src/app/shared/enums/NoteTypes';
import { FolderType } from 'src/app/shared/enums/FolderTypes';
import { AuthService } from '../auth.service';


interface AppState {
    routing: EntityType;
    spinnerActive: boolean;
    token: string;
    tokenUpdated: boolean;
}

@State<AppState>({
    name: 'App',
    defaults: {
        routing: null,
        spinnerActive: false,
        token: null,
        tokenUpdated: false,
    }
})
@Injectable()
export class AppStore {


    constructor(authService: AuthService) {
    }


    @Selector()
    static getToken(state: AppState): string {
        return state.token;
    }

    @Selector()
    static getTokenUpdated(state: AppState): boolean {
        return state.tokenUpdated;
    }

    @Selector()
    static isFolderInner(state: AppState): boolean {
        return state.routing === EntityType.FolderInner;
    }

    @Selector()
    static isNoteInner(state: AppState): boolean {
        return state.routing === EntityType.NoteInner;
    }

    @Selector()
    static spinnerActive(state: AppState): boolean {
        return state.spinnerActive;
    }

    @Selector()
    static isFolder(state: AppState): boolean {
        return state.routing === EntityType.FolderShared ||
        state.routing === EntityType.FolderDeleted ||
        state.routing === EntityType.FolderPrivate ||
        state.routing === EntityType.FolderArchive ||
        state.routing === EntityType.FolderInner;
    }

    @Selector()
    static isNote(state: AppState): boolean {
        return state.routing === EntityType.NoteShared ||
        state.routing === EntityType.NoteDeleted ||
        state.routing === EntityType.NotePrivate ||
        state.routing === EntityType.NoteArchive ||
        state.routing === EntityType.NoteInner;
    }

    @Selector()
    static getName(state: AppState): string {
        switch (state.routing) {

            case EntityType.FolderPrivate: {
                return 'folder';
            }
            case EntityType.FolderShared: {
                return 'folder';
            }
            case EntityType.FolderDeleted: {
                return 'folder';
            }
            case EntityType.FolderArchive: {
                return 'folder';
            }

            case EntityType.NotePrivate: {
                return 'note';
            }
            case EntityType.NoteArchive: {
                return 'note';
            }
            case EntityType.NoteDeleted: {
                return 'note';
            }
            case EntityType.NoteShared: {
                return 'note';
            }

            case EntityType.LabelPrivate: {
                return 'label';
            }
            case EntityType.LabelDeleted: {
                return 'label';
            }

            case EntityType.Profile: {
                return 'background';
            }
        }
    }

    @Selector()
    static getRouting(state: AppState): EntityType {
        return state.routing;
    }


    @Selector()
    static getTypeNote(state: AppState): NoteType {
        switch (state.routing) {
            case EntityType.NotePrivate: {
                return NoteType.Private;
            }
            case EntityType.NoteArchive: {
                return NoteType.Archive;
            }
            case EntityType.NoteDeleted: {
                return NoteType.Deleted;
            }
            case EntityType.NoteShared: {
                return NoteType.Shared;
            }
            default: {
                throw new Error('Incorrect type');
            }
        }
    }

    @Selector()
    static getTypeFolder(state: AppState): FolderType {
        switch (state.routing) {

            case EntityType.FolderPrivate: {
                return FolderType.Private;
            }
            case EntityType.FolderShared: {
                return FolderType.Shared;
            }
            case EntityType.FolderDeleted: {
                return FolderType.Deleted;
            }
            case EntityType.FolderArchive: {
                return FolderType.Archive;
            }
            default: {
                throw new Error('Incorrect type');
            }
        }
    }

    // UPPER MENU SELECTORS

    @Selector()
    static getNewButtonActive(state: AppState): boolean {
        return this.isNote(state) || this.isFolder(state) || state.routing === EntityType.LabelPrivate;
    }

    @Selector()
    static getSettingsButtonActive(state: AppState): boolean {
        return this.isNote(state) || this.isFolder(state);
    }

    @Selector()
    static getSelectAllButtonActive(state: AppState): boolean {
        return  this.isNote(state) || this.isFolder(state);
    }

    @Selector()
    static getDeleteAllLabellsButtonActive(state: AppState): boolean {
        return  state.routing === EntityType.LabelDeleted;
    }

    @Selector()
    static getChangeViewButtonActive(state: AppState): boolean {
        return  state.routing !== EntityType.Profile;
    }

    @Selector()
    static getdefaultBackground(state: AppState): boolean {
        return state.routing === EntityType.Profile;
    }

    @Action(UpdateRoute)
    async updateRoute({patchState}: StateContext<AppState>, {type}: UpdateRoute) {
        patchState({routing: type});
    }

    @Action(SpinnerChangeStatus)
    spinnerChangeStatus({patchState}: StateContext<AppState>, {flag}: SpinnerChangeStatus) {
        patchState({spinnerActive: flag});
    }

    @Action(SetToken)
    setToken({ patchState }: StateContext<AppState>, { token }: SetToken) {
        patchState({ token, tokenUpdated: true });
    }

    @Action(TokenSetNoUpdate)
    setNoUpdateToken({ patchState }: StateContext<AppState>) {
        patchState({  token: null , tokenUpdated: false });
    }
}
