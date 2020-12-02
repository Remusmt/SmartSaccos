import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { Observable, throwError } from 'rxjs';
import { SpinnerService } from './spinner.service';
import { tap, catchError } from 'rxjs/operators';
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService,
                private spinnerService: SpinnerService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        const currentUser = this.authenticationService.currentUserValue;
        if (currentUser && currentUser.tokenString) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser.tokenString}`
                }
            });
        }

        this.spinnerService.show();

        return next.handle(request)
        .pipe(catchError(err => {
          if (err.status === 401) {
              // auto logout if 401 response returned from api
              this.authenticationService.logout();
              location.reload();
          }
          const error = err.error.message || err.statusText;
          return throwError(error);
        }),
          tap((event: HttpEvent<any>) => {
              if (event instanceof HttpResponse) {
                  this.spinnerService.hide();
              }
          }, (error) => {
              this.spinnerService.hide();
          })
      );
    }
}
