import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from './models/User';
import { ShortUser } from './models/ShortUser';
import { AnswerChangePhoto } from './models/AsnwerChangePhoto';
import { UserUsedDiskSpace } from './models/search/UserDiskSpace';
import { ThemeENUM } from '../shared/enums/ThemeEnum';
import { FontSizeENUM } from '../shared/enums/FontSizeEnum';
import { LanguagesENUM } from '../shared/enums/LanguagesENUM';

export interface Token {
  token: string;
}

@Injectable()
export class UserAPIService {
  constructor(private httpClient: HttpClient) {}

  verifyToken(token: string) {
    const value: Token = {
      token,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/auth/verify`, value);
  }

  tryGetFromAuthorize() {
    return this.httpClient.get(`${environment.writeAPI}/api/auth/get`);
  }

  newUser(user: User) {
    return this.httpClient.post<ShortUser>(`${environment.writeAPI}/api/user`, user);
  }

  getUser() {
    return this.httpClient.get<ShortUser>(`${environment.writeAPI}/api/user/short`);
  }

  getMemory() {
    return this.httpClient.get<UserUsedDiskSpace>(`${environment.writeAPI}/api/user/memory`);
  }

  changeTheme(id: ThemeENUM) {
    const obj = {
      id,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/user/theme`, obj);
  }

  changeFontSize(id: FontSizeENUM) {
    const obj = {
      id,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/user/font`, obj);
  }

  changeLanguage(id: LanguagesENUM) {
    const obj = {
      id,
    };
    return this.httpClient.post(`${environment.writeAPI}/api/user/language`, obj);
  }

  updateUserName(name: string) {
    const obj = {
      name,
    };
    return this.httpClient.put(`${environment.writeAPI}/api/user/username`, obj);
  }

  updateUserPhoto(photo: FormData) {
    return this.httpClient.post<AnswerChangePhoto>(`${environment.writeAPI}/api/user/photo`, photo);
  }

  async getImageFromGoogle(imageUrl): Promise<FormData> {
    const imageBlob = await this.httpClient.get(imageUrl, { responseType: 'blob' }).toPromise();
    const form = new FormData();
    form.append('Photo', imageBlob);
    return form;
  }

  // private async getBase64FromBlob(blob: Blob) {
  //   return new Promise<string>((resolve, reject) => {
  //     const reader = new FileReader();
  //     reader.onload = () => {
  //       const dataUrl = reader.result as string;
  //       const base64 = dataUrl.split(',')[1];
  //       resolve(`data:image/jpeg;base64,${base64}`);
  //     };
  //     reader.readAsDataURL(blob);
  //   });
  // }
}
