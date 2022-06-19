import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnChanges,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { combineLatest, Subject } from 'rxjs';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { ExportService } from '../../../../../export.service';
import { ParentInteraction } from '../../../../models/parent-interaction.interface';
import { SelectionService } from '../../../../content-editor-services/selection.service';
import { ClickableContentService } from '../../../../content-editor-services/clickable-content.service';
import { FocusDirection, SetFocus } from '../../../../models/set-focus';
import { ClickableSelectableEntities } from '../../../../content-editor-services/models/clickable-selectable-entities.enum';
import { CollectionBaseComponent } from '../../collection.base.component';
import { Photo, PhotosCollection } from '../../../../../models/editor-models/photos-collection';
import { ApiBrowserTextService } from '../../../../../api-browser-text.service';
@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotosComponent
  extends CollectionBaseComponent<PhotosCollection>
  implements OnInit, OnDestroy, AfterViewInit, OnChanges, ParentInteraction
{
  @ViewChild('album') albumChild: ElementRef;

  @ViewChild('uploadPhotos') uploadPhoto: ElementRef;

  startHeight;

  mainBlocks: Photo[][] = [];

  lastBlock: Photo[] = [];

  destroy = new Subject<void>();

  changeHeightSubject = new Subject<string>();

  changeSizeAlbumHalder = combineLatest([this.changeHeightSubject]);

  constructor(
    private renderer: Renderer2,
    private elRef: ElementRef,
    private selectionService: SelectionService,
    private exportService: ExportService,
    clickableContentService: ClickableContentService,
    private host: ElementRef,
    cdr: ChangeDetectorRef,
    apiBrowserTextService: ApiBrowserTextService,
  ) {
    super(cdr, clickableContentService, apiBrowserTextService);
  }

  get countOfBlocks() {
    return Math.floor(this.content.items.length / this.content.countInRow);
  }

  get countLastItems() {
    return this.content.items.length % this.content.countInRow;
  }

  get totalRows() {
    return this.countLastItems ? this.mainBlocks.length + 1 : this.mainBlocks.length;
  }

  get isEmpty(): boolean {
    if (!this.content.items || this.content.items.length === 0) {
      return true;
    }
    return false;
  }

  ngOnChanges(): void {}

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
  }

  ngAfterViewInit(): void {
    this.setHeight(`${this.content.height}`); // init collection size first
  }

  uploadHandler = () => {
    this.uploadPhoto.nativeElement.click();
  };

  async uploadImages(files: File[]) {
    if (files?.length > 0) {
      this.uploadEvent.emit({ contentId: this.content.id, files: [...files] });
    }
  }

  clickPhotoHandler(photoId: string) {
    this.clickableContentService.setSontent(
      this.content.id,
      photoId,
      ClickableSelectableEntities.Photo,
      null,
    );
  }

  changeHeight(difference: number) {
    const newHeight = this.startHeight + difference;
    if (newHeight > 200) {
      this.setHeight(`${newHeight}px`);
    }
  }

  syncHeight(): void {
    const height = `${this.albumChild?.nativeElement.offsetHeight}px`;
    if (this.content.height !== height) {
      this.setHeight(`${this.content.height}`);
    }
  }

  saveHeight(isResizingPhoto: boolean) {
    this.startHeight = this.albumChild.nativeElement.offsetHeight;
    this.selectionService.isResizingPhoto = isResizingPhoto;
  }

  ngOnInit(): void {
    for (const photo of this.content.items) {
      photo.loaded = false;
    }
    this.changeSizeAlbumHalder
      .pipe(takeUntil(this.destroy), debounceTime(300)) // TODO export const
      .subscribe((values) => {
        const [height] = values;
        if (height && (this.content.height !== height)) {
          this.content.height = height;
          this.someChangesEvent.emit();
        }
      });

    this.changeHeightSubject.next(this.content.height);
    this.initPhotos();
  }

  setPhotosInRowWrapper(count: number): void  {
    this.setPhotosInRow(count);
    this.someChangesEvent.emit();
  }

  setPhotosInRow(count: number): void {
    if (this.content.countInRow === count) return;
    this.content.countInRow = count;
    this.setFalseLoadedForAllPhotos();
    this.setHeight('auto');
    this.initPhotos();
  }

  setFalseLoadedForAllPhotos() {
    for (const mainBlock of this.mainBlocks) {
      mainBlock.forEach((photo) => {
        const item = { ...photo };
        item.loaded = false;
      });
    }
    this.lastBlock.forEach((photo) => {
      const item = { ...photo };
      item.loaded = false;
    });
  }

  async exportAlbum() {
    await this.exportService.exportAlbum(this.content);
  }

  async exportPhoto(photo: Photo) {
    await this.exportService.exportPhoto(photo);
  }

  updateIternal() {
    this.setPhotosInRow(this.content.countInRow);
    this.syncHeight();
  }

  initPhotos() {
    this.content.countInRow = this.content.countInRow === 0 ? 2 : this.content.countInRow;
    this.mainBlocks = [];
    this.lastBlock = [];
    const photoLength = this.content.items.length;
    let j = 0;
    for (let i = 0; i < this.countOfBlocks; i += 1) {
      this.mainBlocks.push(this.content.items.slice(j, j + this.content.countInRow));
      j += this.content.countInRow;
    }
    if (this.countLastItems > 0) {
      this.lastBlock = this.content.items.slice(photoLength - this.countLastItems, photoLength);
    }
  }

  getStyle = (numb: number) => {
    switch (numb) {
      case 1: {
        return 'one-child';
      }
      case 2: {
        return 'two-child';
      }
      case 3: {
        return 'three-child';
      }
      case 4: {
        return 'fouth-child';
      }
      default: {
        return 'fouth-child';
      }
    }
  };

  isFocusToNext(entity: SetFocus) {
    if (entity.status === FocusDirection.Up && this.titleComponent.isFocusedOnTitle) {
      return true;
    }
    if (entity.status === FocusDirection.Down) {
      const index = this.content.items.findIndex((x) => x.fileId === entity.itemId);
      return index === this.content.items.length - 1;
    }
    return false;
  }

  setFocus = (entity?: SetFocus): void => {
    const isExist = this.content.items.some((x) => x.fileId === entity.itemId);

    if (entity.status === FocusDirection.Up && isExist) {
      const index = this.content.items.findIndex((x) => x.fileId === entity.itemId);
      if (index === 0) {
        this.titleComponent.focusOnTitle();
        this.clickPhotoHandler(null);
      } else {
        this.clickPhotoHandler(this.content.items[index - 1].fileId);
        (document.activeElement as HTMLInputElement).blur();
      }
      this.cdr.detectChanges();
      return;
    }

    if (entity.status === FocusDirection.Up && this.content.items.length > 0) {
      this.clickPhotoHandler(this.content.items[this.content.items.length - 1].fileId);
      (document.activeElement as HTMLInputElement).blur();
      this.cdr.detectChanges();
      return;
    }

    if (entity.status === FocusDirection.Up && this.content.items.length === 0) {
      this.titleComponent.focusOnTitle();
      this.clickPhotoHandler(null);
      this.cdr.detectChanges();
      return;
    }

    if (entity.status === FocusDirection.Down && isExist) {
      const index = this.content.items.findIndex((x) => x.fileId === entity.itemId);
      this.clickPhotoHandler(this.content.items[index + 1].fileId);
      (document.activeElement as HTMLInputElement).blur();
      this.cdr.detectChanges();
      return;
    }

    if (entity.status === FocusDirection.Down) {
      if (this.titleComponent.isFocusedOnTitle) {
        // eslint-disable-next-line prefer-destructuring
        this.clickPhotoHandler(this.content.items[0].fileId);
        (document.activeElement as HTMLInputElement).blur();
      } else {
        this.titleComponent.focusOnTitle();
        this.clickPhotoHandler(null);
      }
      this.cdr.detectChanges();
      return;
    }
  };

  setFocusToEnd = () => {};

  getEditableNative = () => {
    return null;
  };

  getHost() {
    return this.host;
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  mouseEnter = ($event: any) => {
    this.isMouseOver = true;
  };

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  mouseLeave = ($event: any) => {
    this.isMouseOver = false;
  };

  // eslint-disable-next-line class-methods-use-this
  backspaceUp() {}

  backspaceDown() {
    this.checkForDelete();
  }

  deleteDown() {
    this.checkForDelete();
  }

  private setHeight(value: string): void {
    this.renderer.setStyle(this.albumChild.nativeElement, 'height', value);
    this.changeHeightSubject.next(value);
  }
}
