import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CurrentUser } from '../models/current-user';
import { LoginModel } from '../models/login-model';
import { ConstantsService } from './constants.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private currentUserSubject: BehaviorSubject<CurrentUser>;
  public currentUser: Observable<CurrentUser>;
  constructor(private http: HttpClient,
              private constants: ConstantsService) {

    this.currentUserSubject = new BehaviorSubject<CurrentUser>
        (JSON.parse(localStorage.getItem('currentMember') as string));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): CurrentUser {
    return this.currentUserSubject.value;
  }

  public setUser(user: CurrentUser): void {
    localStorage.setItem('currentMember', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  login(model: LoginModel): Observable<CurrentUser> {
      return this.http.post<CurrentUser>(`${this.constants.baseUrl}Accounts/Login`, model)
          .pipe(map(user => {
              // store user details and jwt token in local storage to keep user logged in between page refreshes
              localStorage.setItem('currentMember', JSON.stringify(user));
              this.currentUserSubject.next(user);
              return user;
          }));
  }

  logout(): void {
      // remove user from local storage and set current user to null
      localStorage.removeItem('currentMember');
      this.currentUserSubject.next(null as any);
      location.reload();
  }
}
