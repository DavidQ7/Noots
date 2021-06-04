import { LanguageDTO } from 'src/app/shared/models/LanguageDTO';
import { Theme } from 'src/app/shared/models/Theme';
import { FontSize } from 'src/app/shared/models/FontSize';
import { Background } from '../models/background';
import { User } from '../models/user';

export class Login {
  static type = '[User] Login User';

  constructor(public token: string, public user: User) {}
}

export class Logout {
  static type = '[User] Logout User';
}

export class ChangeTheme {
  static type = '[User] Change theme';

  constructor(public theme: Theme) {}
}

export class ChangeLanguage {
  static type = '[User] Change Language';

  constructor(public language: LanguageDTO) {}
}

export class ChangeFontSize {
  static type = '[User] Change FontSize';

  constructor(public fontSize: FontSize) {}
}

export class SetCurrentBackground {
  static type = '[User] setCurrent Background';

  constructor(public background: Background) {}
}

export class SetDefaultBackground {
  static type = '[User] setDefault Background';
}

export class UpdateUserName {
  static type = '[User] update userName';

  constructor(public newName: string) {}
}

export class UpdateUserPhoto {
  static type = '[User] update userPhoto';

  constructor(public photo: FormData) {}
}

export class LoadUsedDiskSpace {
  static type = '[User] load used disk space';

  constructor() {}
}
