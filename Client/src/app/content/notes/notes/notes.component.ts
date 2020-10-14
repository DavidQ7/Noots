import { Component, OnInit, OnDestroy, ViewChild, ElementRef, } from '@angular/core';
import { Theme } from 'src/app/shared/enums/Theme';
import { PersonalizationService, sideBarCloseOpen } from 'src/app/shared/services/personalization.service';
import { Subject, Observable } from 'rxjs';
import { takeUntil, take, } from 'rxjs/operators';
import { Select, Store } from '@ngxs/store';
import { LabelStore } from '../../labels/state/labels-state';
import { Label } from '../../labels/models/label';
import { LoadLabels } from '../../labels/state/labels-actions';
import { AddNote } from '../state/notes-actions';
import { Router } from '@angular/router';
import { NoteStore } from '../state/notes-state';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { PaginationService } from 'src/app/shared/services/pagination.service';

export enum subMenu {
  All = 'all',
  Shared = 'shared',
  Locked = 'locked',
  Archive = 'archive',
  Bin = 'bin'
}


@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss'],
  animations: [ sideBarCloseOpen ]
})

export class NotesComponent implements OnInit, OnDestroy {

  @ViewChild ('scrollMe', { static: true })
  public myScrollContainer: ElementRef;

  destroy = new Subject<void>();
  loaded = false;
  theme = Theme;

  labelsActive: number[] = [];
  actives = new Map<number, boolean>();

  @Select(UserStore.getUserTheme)
  public theme$: Observable<Theme>;

  @Select(LabelStore.all)
  public labels$: Observable<Label[]>;


  @Select(NoteStore.privateCount)
  public countPrivate: Observable<number>;

  @Select(NoteStore.sharedCount)
  public countShared: Observable<number>;

  @Select(NoteStore.deletedCount)
  public countDeleted: Observable<number>;

  @Select(NoteStore.archiveCount)
  public countArchive: Observable<number>;

  constructor(public pService: PersonalizationService,
              private store: Store,
              private router: Router,
              private pagService: PaginationService) { }

  async ngOnInit() {
    this.store.select(UserStore.getTokenUpdated)
    .pipe(takeUntil(this.destroy))
    .subscribe(async (x: boolean) => {
      if (x) {
        this.store.dispatch(new LoadLabels());
        this.loaded =  await this.initPromise();
      }
    });

    this.pService.subject
    .pipe(takeUntil(this.destroy))
    .subscribe(x => this.newNote());

    this.pService.onResize();

    setTimeout(() => {
      (this.myScrollContainer as any).SimpleBar.getScrollElement().addEventListener('scroll',
      (e) => {
        const flag = e.srcElement.scrollHeight - e.srcElement.scrollTop - this.pagService.startPointToGetData <= e.srcElement.clientHeight;
        if (flag && !this.pagService.set.has(e.srcElement.scrollHeight)) {
          this.pagService.set.add(e.srcElement.scrollHeight);
          this.pagService.nextPagination.next();
        }
      }); }, 0);
  }

  initPromise() {
    return new Promise<boolean>((resolve, rej) => setTimeout(() => resolve(true), this.pService.timeForLabelsLoading));
  }

  async newNote() {
    await this.store.dispatch(new AddNote()).toPromise();
    this.store.select(state => state.Notes.privateNotes).pipe(take(1)).subscribe(x => this.router.navigate([`notes/${x[0].id}`]));
  }

  cancelLabel() {
    this.labelsActive = [];
    this.actives = new Map();
  }

  cancelAdd(id: number) {
    const flag = (this.actives.get(id) === undefined) || (this.actives.get(id) === false) ? true : false;
    this.actives.set(id, flag);
    if (flag) {
      this.labelsActive.push(id);
    } else {
      this.labelsActive = this.labelsActive.filter(x => x !== id);
    }
  }

  cancelSideBar() {
    this.pService.stateSidebar = false;
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
  }
}
