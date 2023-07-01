import { ConnectionPositionPair } from '@angular/cdk/overlay';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { Observable, Subject } from 'rxjs';
import { AppStore } from 'src/app/core/stateApp/app-state';
import { ChangeTheme } from 'src/app/core/stateUser/user-action';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { ThemeENUM } from 'src/app/shared/enums/theme.enum';
import {
  notification,
  PersonalizationService,
} from 'src/app/shared/services/personalization.service';
import { ContactUsComponent } from '../../../../shared/modal_components/contact-us/contact-us.component';
import { DialogsManageService } from '../../services/dialogs-manage.service';

@Component({
  selector: 'app-interaction-tools',
  templateUrl: './interaction-tools.component.html',
  styleUrls: ['./interaction-tools.component.scss'],
  animations: [notification],
})
export class InteractionToolsComponent implements OnInit, OnDestroy {
  @Select(AppStore.getNotificationsCount)
  public notificationCount$: Observable<number>;

  @Select(AppStore.isNoteInner)
  public isNoteInner$: Observable<boolean>;

  @Select(AppStore.isProfile)
  public isProfile$: Observable<boolean>;

  @Select(UserStore.getUserTheme)
  public theme$: Observable<ThemeENUM>;

  isOpenNotification = false;

  searchStr: string;

  theme = ThemeENUM;

  destroy = new Subject<void>();

  public positions = [
    new ConnectionPositionPair(
      {
        originX: 'end',
        originY: 'bottom',
      },
      { overlayX: 'end', overlayY: 'top' },
      59,
      10,
    ),
  ];

  constructor(
    public readonly pService: PersonalizationService,
    private readonly store: Store,
    private readonly router: Router,
    private readonly dialog: MatDialog,
    private readonly dialogsManageService: DialogsManageService,
  ) {}

  ngOnDestroy() {
    this.destroy.next();
    this.destroy.complete();
  }

  ngOnInit(): void {}

  closeNotification() {
    this.isOpenNotification = false;
  }

  toggleOrientation() {
    this.pService.changeOrientation();
  }

  toggleTheme() {
    const userTheme = this.store.selectSnapshot(UserStore.getUserTheme);
    if (userTheme === ThemeENUM.Dark) {
      this.store.dispatch(new ChangeTheme(ThemeENUM.Light));
    }
    if (userTheme === ThemeENUM.Light) {
      this.store.dispatch(new ChangeTheme(ThemeENUM.Dark));
    }
  }

  contactUs() {
    const theme = this.store.selectSnapshot(UserStore.getUserTheme);
    const config: MatDialogConfig = {
      maxHeight: '90vh',
      maxWidth: '90vw',
      autoFocus: false,
      panelClass:
        theme === ThemeENUM.Light ? 'custom-dialog-class-light' : 'custom-dialog-class-dark',
    };
    this.dialog.open(ContactUsComponent, config);
  }

  openSearch(): void {
    this.dialogsManageService.openSearchDialog();
  }

  goTo(id: string, type: string) {
    if (type === 'notes') {
      this.router.navigateByUrl(`notes/${id}`);
      return;
    }
    this.router.navigateByUrl(`folders/${id}`);
  }
}
