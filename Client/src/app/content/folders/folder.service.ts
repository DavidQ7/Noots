import { ElementRef, Injectable, OnDestroy, QueryList } from '@angular/core';
import { Actions, ofActionDispatched, Store } from '@ngxs/store';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { MurriService } from 'src/app/shared/services/murri.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FolderTypeENUM } from 'src/app/shared/enums/folder-types.enum';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { SortedByENUM } from 'src/app/core/models/sorted-by.enum';
import { AppStore } from 'src/app/core/stateApp/app-state';
import { FeaturesEntitiesService } from 'src/app/shared/services/features-entities.service';
import { UpdaterEntitiesService } from 'src/app/core/entities-updater.service';
import { SmallFolder } from './models/folder.model';
import { FolderStore } from './state/folders-state';
import {
  ClearAddToDomFolders,
  ClearUpdatesUIFolders,
  LoadFolders,
  UpdateFolderTitleWS,
  UpdateOneFolder,
  UpdatePositionsFolders,
} from './state/folders-actions';
import { ApiFoldersService } from './api-folders.service';
import { FolderComponent } from './folder/folder.component';
import { UpdateFolderUI } from './state/update-folder-ui.model';

/** Injection only in component */
@Injectable()
export class FolderService extends FeaturesEntitiesService<SmallFolder> implements OnDestroy {
  viewElements: QueryList<FolderComponent>;

  destroy = new Subject<void>();

  prevSortedFolderByTypeId: SortedByENUM = null;

  constructor(
    store: Store,
    public pService: PersonalizationService,
    murriService: MurriService,
    private updateService: UpdaterEntitiesService,
    private apiFolders: ApiFoldersService,
    public actions$: Actions,
  ) {
    super(store, murriService);

    this.store
      .select(FolderStore.updateFolderEvents)
      .pipe(takeUntil(this.destroy))
      .subscribe(async (values) => {
        await this.updateFolders(values);
      });

    this.actions$
      .pipe(takeUntil(this.destroy), ofActionDispatched(UpdateFolderTitleWS))
      .subscribe((value: UpdateFolderTitleWS) => {
        const viewChid = this.viewElements.toArray().find((x) => x.folder.id === value.folderId);
        if (viewChid) {
          viewChid.updateRGATitle(value.transactions);
        }
      });

    this.store
      .select(FolderStore.removeFromMurriEvent)
      .pipe(takeUntil(this.destroy))
      .subscribe((x) => this.deleteFromDom(x));

    this.store
      .select(FolderStore.selectedIds)
      .pipe(takeUntil(this.destroy))
      .subscribe((ids) => this.handleSelectEntities(ids));

    this.store
      .select(FolderStore.selectedCount)
      .pipe(takeUntil(this.destroy))
      .subscribe((count) => this.handleLockRedirect(count));

    this.store
      .select(UserStore.getPersonalizationSettings)
      .pipe(takeUntil(this.destroy))
      .subscribe(async (pr) => {
        if (
          this.prevSortedFolderByTypeId &&
          this.prevSortedFolderByTypeId !== pr.sortedFolderByTypeId
        ) {
          this.prevSortedFolderByTypeId = pr.sortedFolderByTypeId;
          await this.changeOrderTypeHandler(this.pageSortType);
        } else {
          this.prevSortedFolderByTypeId = pr.sortedFolderByTypeId;
        }
      });

    this.store
      .select(FolderStore.foldersAddToDOM)
      .pipe(takeUntil(this.destroy))
      .subscribe((x) => {
        if (x.length > 0) {
          this.addToDom(x);
          this.store.dispatch(ClearAddToDomFolders);
        }
      });
  }

  get isAnySelected(): boolean {
    return this.entities.some((z) => z.isSelected === true);
  }

  get getByCurrentType() {
    switch (this.store.selectSnapshot(AppStore.getTypeFolder)) {
      case FolderTypeENUM.Private: {
        return this.store.selectSnapshot(FolderStore.privateFolders);
      }
      case FolderTypeENUM.Shared: {
        return this.store.selectSnapshot(FolderStore.sharedFolders);
      }
      case FolderTypeENUM.Archive: {
        return this.store.selectSnapshot(FolderStore.archiveFolders);
      }
      case FolderTypeENUM.Deleted: {
        return this.store.selectSnapshot(FolderStore.deletedFolders);
      }
      default: {
        throw new Error('Incorrect type');
      }
    }
  }

  get isSortable() {
    return this.sortFolderType === SortedByENUM.CustomOrder;
  }

  get sortFolderType() {
    return this.store.selectSnapshot(UserStore.getPersonalizationSettings).sortedFolderByTypeId;
  }

  get pageSortType(): SortedByENUM {
    const isSharedType =
      this.store.selectSnapshot(AppStore.getTypeFolder) === FolderTypeENUM.Shared;
    if (isSharedType) {
      return SortedByENUM.DescDate;
    }
    return this.sortFolderType;
  }

  async updateFolders(updates: UpdateFolderUI[]) {
    for (const value of updates) {
      const folder = this.entities.find((x) => x.id === value.id) as SmallFolder;
      if (folder !== undefined) {
        folder.color = value.color ?? folder.color;
        folder.isCanEdit = value.isCanEdit ?? folder.isCanEdit;
      }
    }
    if (updates.length > 0) {
      await this.store.dispatch(new ClearUpdatesUIFolders()).toPromise();
      await this.murriService.refreshLayoutAsync();
    }
  }

  updatePositions(): void {
    this.store.dispatch(new UpdatePositionsFolders(this.murriService.getPositions()));
  }

  ngOnDestroy(): void {
    console.log('folder destroy');
    super.destroyLayout();
    this.destroy.next();
    this.destroy.complete();
  }

  murriInitialise(refElements: QueryList<ElementRef>, isDragEnabled: boolean = true) {
    refElements.changes.pipe(takeUntil(this.destroy)).subscribe(async (q) => {
      if (this.getIsFirstInit(q)) {
        // eslint-disable-next-line no-param-reassign
        isDragEnabled = isDragEnabled && this.isSortable;
        this.murriService.initMurriFolder(isDragEnabled);
        await this.setInitMurriFlagShowLayout();
        await this.loadWithUpdates();
      }
      await this.synchronizeState(refElements, this.sortFolderType === SortedByENUM.AscDate);
    });
  }

  async loadWithUpdates() {
    const pr = this.store.selectSnapshot(UserStore.getPersonalizationSettings);
    this.updateService.foldersIds$.pipe(takeUntil(this.destroy)).subscribe(async (ids) => {
      if (ids.length > 0) {
        const folders = await this.apiFolders.getFoldersMany(ids, pr).toPromise();
        const actionsForUpdate = folders.map((folder) => new UpdateOneFolder(folder, folder.id));
        this.store.dispatch(actionsForUpdate);
        const transformFolders = this.transformSpread(folders);
        transformFolders.forEach((folder) => {
          const folderFinded = this.entities.find((x) => x.id === folder.id);
          if (folderFinded) {
            folderFinded.previewNotes = folder.previewNotes;
            this.loadAdditionInformation(ids);
          }
        });
        await this.murriService.refreshLayoutAsync();
        this.updateService.foldersIds$.next([]);
      }
    });
  }

  async loadAdditionInformation(folderIds?: string[]) {
    folderIds = folderIds ?? this.entities.map((x) => x.id);
    if (folderIds.length > 0) {
      const additionalInfo = await this.apiFolders.getAdditionalInfos(folderIds).toPromise();
      for (const info of additionalInfo) {
        const index = this.entities.findIndex((x) => x.id === info.folderId);
        if (index !== -1) {
          this.entities[index].additionalInfo = info;
        }
      }
    }
  }

  async changeOrderTypeHandler(sortType: SortedByENUM) {
    await this.murriService.destroyGridAsync();
    this.entities = this.orderBy(this.entities, sortType);
    const roadType = this.store.selectSnapshot(AppStore.getTypeFolder);
    const isDraggable = roadType !== FolderTypeENUM.Shared && this.isSortable;
    this.murriService.initMurriFolderAsync(isDraggable);
    await this.murriService.setOpacityFlagAsync(0);
  }

  async loadFolders(typeENUM: FolderTypeENUM) {
    const pr = this.store.selectSnapshot(UserStore.getPersonalizationSettings);
    await this.store.dispatch(new LoadFolders(typeENUM, pr)).toPromise();

    const types = Object.values(FolderTypeENUM).filter(
      (q) => typeof q === 'number' && q !== typeENUM,
    );
    const actions = types.map((t: FolderTypeENUM) => new LoadFolders(t, pr));
    this.store.dispatch(actions);
  }

  async initializeEntities(folders: SmallFolder[]) {
    const tempFolders = this.transformSpread(folders);
    this.entities = this.orderBy(tempFolders, this.pageSortType);
    super.initState();

    await this.loadAdditionInformation();
  }
}
