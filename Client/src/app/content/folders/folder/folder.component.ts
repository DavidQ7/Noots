import { Component, OnInit, Input, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime } from 'rxjs/operators';
import { Router } from '@angular/router';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { FontSizeENUM } from 'src/app/shared/enums/font-size.enum';
import { updateTitleEntitesDelay } from 'src/app/core/defaults/bounceDelay';
import { SelectIdFolder, UnSelectIdFolder, UpdateFolderTitle } from '../state/folders-actions';
import { FolderStore } from '../state/folders-state';
import { SmallFolder } from '../models/folder.model';
import { FolderTypeENUM } from 'src/app/shared/enums/folder-types.enum';
import * as moment from 'moment';
import { ApiBrowserTextService } from '../../notes/api-browser-text.service';
import { ContentEditableService } from '../../notes/full-note/content-editor-services/content-editable.service';
import { DiffCheckerService } from '../../notes/full-note/content-editor/diffs/diff-checker.service';

@Component({
  selector: 'app-folder',
  templateUrl: './folder.component.html',
  styleUrls: ['./folder.component.scss'],
})
export class FolderComponent implements OnInit, OnDestroy {
  @Input() folder: SmallFolder;

  @ViewChild('folderTitle', { read: ElementRef }) folderTitleEl: ElementRef<HTMLInputElement>;

  @Input() date: string;

  @Input() isShowWrightRead = false;

  @Input() tooltipDateMessage: string;

  @Input() isSelectedMode: boolean;

  @Input() userId: string;

  fontSize = FontSizeENUM;

  destroy = new Subject<void>();

  folderType = FolderTypeENUM;

  // TITLE
  uiTitle: string;

  titleChange$: Subject<string> = new Subject<string>();
  //

  constructor(
    private store: Store,
    private router: Router,
    public pService: PersonalizationService,
    private diffCheckerService: DiffCheckerService,
    private apiBrowserFunctions: ApiBrowserTextService,
    private contentEditableService: ContentEditableService,
  ) {}

  get isAuthor(): boolean {
    return this.userId === this.folder.userId;
  }

  get updateTime() {
    return moment(this.date ?? this.folder.updatedAt).format('DD.MM hh:mm');
  }

  setTitle(): void {
    // SET
    this.uiTitle = this.folder.title;

    // UPDATE CURRENT
    this.titleChange$
      .pipe(takeUntil(this.destroy), debounceTime(updateTitleEntitesDelay))
      .subscribe((title) => {
        const diffs = this.diffCheckerService.getDiffs(this.folder.title, title);
        this.store.dispatch(
          new UpdateFolderTitle(diffs, title, this.folder.id, true, null, false, false),
        );
        this.folder.title = title;
      });
  }

  updateTitle(title: string): void {
    if (this.folder.title !== title && this.folderTitleEl?.nativeElement) {
      const pos = this.apiBrowserFunctions.getSelectionCharacterOffsetsWithin(
        this.folderTitleEl?.nativeElement,
      );
      this.uiTitle = title;
      this.folder.title = title;
      requestAnimationFrame(() =>
        this.contentEditableService.setCaret(this.folderTitleEl?.nativeElement, pos?.start),
      );
    }
  }

  ngOnInit(): void {
    this.setTitle();
  }

  tryFind(z: string[]): boolean {
    const exist = z.find((id) => id === this.folder.id);
    return exist !== undefined;
  }

  handelEntityClick($event: MouseEvent, id: string) {
    if (this.isSelectedMode) {
      this.highlight(id);
    }
  }

  highlight(id: string) {
    const ids = this.store.selectSnapshot(FolderStore.selectedIds);
    if (!this.folder.isSelected) {
      this.store.dispatch(new SelectIdFolder(id, ids));
    } else {
      this.store.dispatch(new UnSelectIdFolder(id, ids));
    }
  }

  toFolder($event: MouseEvent) {
    $event.stopPropagation();
    $event.preventDefault();
    const flag = this.store.selectSnapshot(FolderStore.selectedCount) > 0;
    if (flag) {
      this.highlight(this.folder.id);
    } else {
      this.router.navigate([`folders/${this.folder.id}`]);
    }
  }

  shadeColor = (color, percent) => {
    let R = parseInt(color.substring(1, 3), 16);
    let G = parseInt(color.substring(3, 5), 16);
    let B = parseInt(color.substring(5, 7), 16);

    R = parseInt(((R * (100 + percent)) / 100).toString(), 10);
    G = parseInt(((G * (100 + percent)) / 100).toString(), 10);
    B = parseInt(((B * (100 + percent)) / 100).toString(), 10);

    R = R < 255 ? R : 255;
    G = G < 255 ? G : 255;
    B = B < 255 ? B : 255;

    const RR = R.toString(16).length === 1 ? `0${R.toString(16)}` : R.toString(16);
    const GG = G.toString(16).length === 1 ? `0${G.toString(16)}` : G.toString(16);
    const BB = B.toString(16).length === 1 ? `0${B.toString(16)}` : B.toString(16);

    return `#${RR}${GG}${BB}`;
  };

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
  }
}
