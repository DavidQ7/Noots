import { Component, HostBinding, Input } from '@angular/core';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ShortUser } from 'src/app/core/models/user/short-user.model';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { ThemeENUM } from '../enums/theme.enum';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss'],
})
export class SpinnerComponent {
  @Select(UserStore.getUser)
  public user$?: Observable<ShortUser>;

  @Select(UserStore.getUserTheme)
  public theme$?: Observable<ThemeENUM>;

  theme = ThemeENUM;

  @Input() size?: number;

  @Input() isBackground = true;

  @HostBinding('style.--target-color')
  @Input()
  color?: string;
}
