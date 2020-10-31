import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { Theme } from 'src/app/shared/enums/Theme';
import { PersonalizationService, sideBarCloseOpen } from 'src/app/shared/services/personalization.service';
import { Subject, Observable } from 'rxjs';
import { takeUntil, take } from 'rxjs/operators';
import { Folder } from '../models/folder';
import { FolderStore } from '../state/folders-state';
import { Select, Store } from '@ngxs/store';
import { AddFolder } from '../state/folders-actions';
import { Router } from '@angular/router';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { PaginationService } from 'src/app/shared/services/pagination.service';
import { ShortUser } from 'src/app/core/models/short-user';
import { AppStore } from 'src/app/core/stateApp/app-state';


@Component({
  selector: 'app-folders',
  templateUrl: './folders.component.html',
  styleUrls: ['./folders.component.scss'],
  animations: [ sideBarCloseOpen ]
})
export class FoldersComponent implements OnInit, OnDestroy {

  @ViewChild ('scrollMe', { static: true })
  public myScrollContainer: ElementRef;

  @Select(AppStore.spinnerActive)
  public spinnerActive$: Observable<boolean>;

  @Select(UserStore.getUserTheme)
  public theme$: Observable<Theme>;

  @Select(FolderStore.privateCount)
  public countPrivate: Observable<number>;

  @Select(FolderStore.sharedCount)
  public countShared: Observable<number>;

  @Select(FolderStore.deletedCount)
  public countDeleted: Observable<number>;

  @Select(FolderStore.archiveCount)
  public countArchive: Observable<number>;

  @Select(UserStore.getUser)
  public user$: Observable<ShortUser>;


  destroy = new Subject<void>();

  theme = Theme;
  public photoError = false;
  constructor(public pService: PersonalizationService,
              private store: Store,
              private router: Router,
              private pagService: PaginationService) { }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
  }

  async ngOnInit() {

    this.pService.onResize();
    this.pService.subject
    .pipe(takeUntil(this.destroy))
    .subscribe(x => this.newFolder());

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

  async newFolder() {
    await this.store.dispatch(new AddFolder()).toPromise();
    this.store.select(FolderStore.privateFolders).pipe(take(1)).subscribe(x => this.router.navigate([`folders/${x[0].id}`]));
  }

  changeSource(event) {
    this.photoError = true;
  }
}
