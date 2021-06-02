import { FontSize } from 'src/app/shared/models/FontSize';
import { LanguageDTO } from 'src/app/shared/models/LanguageDTO';
import { Theme } from 'src/app/shared/models/Theme';
import { Background } from './background';

export interface ShortUser {
  id: string;
  name: string;
  email: string;
  photoId: string;
  photoPath: string;
  currentBackground: Background;
  language: LanguageDTO;
  theme: Theme;
  fontSize: FontSize;
}
