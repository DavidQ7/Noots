import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { UpdateRoute, SetToken, TokenSetNoUpdate, LoadLanguages, LoadThemes, LoadFontSizes } from './app-action';
import { EntityType } from 'src/app/shared/enums/EntityTypes';
import { NoteType } from 'src/app/shared/enums/NoteTypes';
import { FolderType } from 'src/app/shared/enums/FolderTypes';
import { AppServiceAPI } from '../app.service';
import { LanguageDTO } from 'src/app/shared/models/Language';
import { AuthService } from '../auth.service';
import { Theme } from 'src/app/shared/models/Theme';
import { FontSize } from 'src/app/shared/models/FontSize';


interface AppState {
    routing: EntityType;
    token: string;
    tokenUpdated: boolean;
    languages: LanguageDTO[];
    themes: Theme[];
    fontSizes: FontSize[];
}

@State<AppState>({
    name: 'App',
    defaults: {
        routing: null,
        token: null,
        tokenUpdated: false,
        languages: [],
        themes: [],
        fontSizes: []
    }
})
@Injectable()
export class AppStore {


    constructor(
        authService: AuthService, // DONT DELETE THIS ROW
        public appService: AppServiceAPI) {
    }

    @Selector()
    static getLanguages(state: AppState): LanguageDTO[] {
        return state.languages;
    }


    @Selector()
    static getThemes(state: AppState): Theme[] {
        return state.themes;
    }

    @Selector()
    static getFontSizes(state: AppState): FontSize[] {
        return state.fontSizes;
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
    static isDelete(state: AppState): boolean {
        return state.routing === EntityType.NoteDeleted ||
        state.routing === EntityType.FolderDeleted;
    }

    @Selector()
    static isProfile(state: AppState): boolean {
        return state.routing === EntityType.Profile;
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
        return !this.isNoteInner(state) &&
        !this.isFolderInner(state) &&
        state.routing !== EntityType.LabelDeleted &&
        state.routing !== null;
    }

    @Action(UpdateRoute)
    async updateRoute({patchState}: StateContext<AppState>, {type}: UpdateRoute) {
        patchState({routing: type});
    }

    @Action(SetToken)
    setToken({ patchState }: StateContext<AppState>, { token }: SetToken) {
        patchState({ token, tokenUpdated: true });
    }

    @Action(TokenSetNoUpdate)
    setNoUpdateToken({ patchState }: StateContext<AppState>) {
        patchState({  token: null , tokenUpdated: false });
    }

    @Action(LoadLanguages)
    async loadLanguages({ patchState }: StateContext<AppState>) {
        const languages = await this.appService.getLanguages().toPromise();
        patchState({  languages });
    }

    @Action(LoadThemes)
    async loadThemes({ patchState }: StateContext<AppState>) {
        const themes = await this.appService.getThemes().toPromise();
        patchState({  themes });
    }

    @Action(LoadFontSizes)
    async loadFontSizes({ patchState }: StateContext<AppState>) {
        const fontSizes = await this.appService.getFontSizes().toPromise();
        patchState({  fontSizes });
    }


}
