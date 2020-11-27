import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as firebase from 'firebase';
import { AngularFireAuth } from '@angular/fire/auth';
import { UserAPIService } from './user-api.service';
import { User } from './models/user';
import { Language } from '../shared/enums/Language';
import { Store } from '@ngxs/store';
import { Login, Logout } from './stateUser/user-action';
import { UserStore } from './stateUser/user-state';
import { SetToken } from './stateApp/app-action';


@Injectable()
export class AuthService {

  constructor(
    private afAuth: AngularFireAuth,
    private router: Router,
    private store: Store,
    private api: UserAPIService) {
      this.afAuth.authState.subscribe(async (firebaseUser) => {
      await this.configureAuthState(firebaseUser);
    });
  }

  GoogleAuth() {
    return this.AuthLogin(new firebase.auth.GoogleAuthProvider());
  }

  private async configureAuthState(firebaseUser: firebase.User) {
    if (firebaseUser) {
      const token = await firebaseUser.getIdToken(true);
      await this.api.verifyToken(token).toPromise();
      this.store.dispatch(new SetToken(token));
      const flag = this.store.selectSnapshot(UserStore.getStatus);
      if (!flag) {
        const user = this.getUser(firebaseUser);
        this.store.dispatch(new Login(token, user)).subscribe(x => this.router.navigate(['/notes']));
      }
      setInterval(async () => await this.updateToken(firebaseUser), 10 * 60 * 1000);
    } else {
      await this.logout();
    }
  }

  private async updateToken(firebaseUser: firebase.User) {
    const token = await firebaseUser.getIdToken(true);
    await this.api.verifyToken(token).toPromise();
    this.store.dispatch(new SetToken(token));
  }

  private getUser(user: firebase.User) {
    const temp: User = {
      name: user.displayName,
      photoId: user.photoURL,
      language: Language.UA
    };
    return temp;
  }

  private AuthLogin(provider: firebase.auth.GoogleAuthProvider) {
    return this.afAuth.signInWithRedirect(provider)
      .then(result => { })
      .catch(error => {
        window.alert(error);
      });
  }

  async logout() {
    await this.store.dispatch(new Logout()).toPromise();
    await this.afAuth.signOut();
    await this.router.navigate(['/about']);
  }
}
