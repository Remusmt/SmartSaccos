import { map } from 'rxjs/operators';
import { AuthenticationService } from './../shared/services/authentication.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ConstantsService } from './../shared/services/constants.service';
import { Injectable } from '@angular/core';
import { RegisterMember } from '../models/register-member';
import { CurrentUser } from '../shared/models/current-user';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(
    private http: HttpClient,
    private constants: ConstantsService,
    private authenticationService: AuthenticationService) { }

    Register(model: RegisterMember): Observable<CurrentUser> {
      return this.http.post<CurrentUser>(this.constants.baseUrl + 'Accounts/Register', model)
      .pipe(
        map(res => {
          this.authenticationService.setUser(res);
          return res;
        })
      );
    }

}
