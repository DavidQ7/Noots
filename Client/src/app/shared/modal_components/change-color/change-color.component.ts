import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DialogData } from '../dialog_data';
import { EnumUtil } from '../../services/enum.util';
import { NoteColorPallete } from '../../enums/NoteColors';
import { PersonalizationService } from '../../services/personalization.service';
import { Theme } from '../../enums/Theme';
import { Store, Select } from '@ngxs/store';
import { ChangeColorNote, UnSelectAllNote } from 'src/app/content/notes/state/notes-actions';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { Observable } from 'rxjs/internal/Observable';
import { AppStore } from 'src/app/core/stateApp/app-state';
import { ChangeColorFolder } from 'src/app/content/folders/state/folders-actions';

@Component({
  selector: 'app-change-color',
  templateUrl: './change-color.component.html',
  styleUrls: ['./change-color.component.scss']
})
export class ChangeColorComponent implements OnInit, OnDestroy {

  @Select(AppStore.isNote)
  public isNote$: Observable<boolean>;

  @Select(AppStore.isFolder)
  public isFolder$: Observable<boolean>;



  @Select(UserStore.getUserTheme)
  public theme$: Observable<Theme>;

  pallete = EnumUtil.getEnumValues(NoteColorPallete);
  current;
  theme = Theme;

  date: Date;
  constructor(public dialogRef: MatDialogRef<ChangeColorComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private store: Store, ) { }

  ngOnInit(): void {
    this.date = new Date();
    this.current = this.pallete[0];
  }

  changeCurrent(color: string) {
    this.current = color;
  }

  async changeColor() { // TODO
    let routePath = this.store.selectSnapshot(AppStore.isNote);
    if (routePath) {
      const type = this.store.selectSnapshot(AppStore.getTypeNote);
      await this.store.dispatch(new ChangeColorNote(this.current, type)).toPromise();
    }
    routePath = this.store.selectSnapshot(AppStore.isFolder);
    if (routePath) {
      const type = this.store.selectSnapshot(AppStore.getRouting);
      await this.store.dispatch(new ChangeColorFolder(this.current, type)).toPromise();
    }
    this.dialogRef.close();
  }

  shadeColor(color, percent) {

    let R = parseInt(color.substring(1, 3), 16);
    let G = parseInt(color.substring(3, 5), 16);
    let B = parseInt(color.substring(5, 7), 16);

    R = parseInt((R * (100 + percent) / 100).toString(), 10);
    G = parseInt((G * (100 + percent) / 100).toString(), 10);
    B = parseInt((B * (100 + percent) / 100).toString(), 10);

    R = (R < 255) ? R : 255;
    G = (G < 255) ? G : 255;
    B = (B < 255) ? B : 255;

    const RR = ((R.toString(16).length === 1) ? '0' + R.toString(16) : R.toString(16));
    const GG = ((G.toString(16).length === 1) ? '0' + G.toString(16) : G.toString(16));
    const BB = ((B.toString(16).length === 1) ? '0' + B.toString(16) : B.toString(16));

    return '#' + RR + GG + BB;
  }

  ngOnDestroy(): void {
    const flag = this.store.selectSnapshot(AppStore.isNoteInner);
    if (!flag) {
    this.store.dispatch(new UnSelectAllNote());
    }
  }
}
