import { Injectable } from '@angular/core';
import { CanActivate, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';
import { UserStore } from '../stateUser/user-state';

@Injectable()
export class ContentActiveteGuard implements CanActivate {
  constructor(private router: Router, private store: Store) {}

  // eslint-disable-next-line consistent-return
  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const flag = this.store.selectSnapshot(UserStore.getStatus);
    if (flag) {
      return flag;
    }
    this.router.navigate(['/about']);
  }
}