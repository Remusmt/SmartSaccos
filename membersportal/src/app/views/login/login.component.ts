import { Subscription } from 'rxjs';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginModel } from 'src/app/shared/models/login-model';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  formGroup: FormGroup|any;
  serverError = '';
  returnUrl = '';
  subscription: Subscription = new Subscription();
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private userService: AuthenticationService
  ) {
      this.formGroup = this.fb.group({
        email: new FormControl('',
        [
          Validators.required,
          Validators.email
        ]),
        password: new FormControl('',
        [
          Validators.required
        ])
      });
   }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    if (this.userService.currentUserValue) {
      this.router.navigate(['/home']);
    }
    this.subscription.add(
      this.route.queryParams
      .subscribe(
        res => {
          this.returnUrl = res.returnUrl;
        }
      )
    );
  }

  getEmailErrors(): string {
    if (this.formGroup.get('email').hasError('required')) {
      return 'Email is required';
    }

    if (this.formGroup.get('email').hasError('email')) {
      return 'Invalid email';
    }
    return '';
  }

  getPasswordErrors(): string {
    if (this.formGroup.get('password').hasError('required')) {
      return 'Password is required';
    }
    return '';
  }

  submitForm(postModel: LoginModel): void {
    if (this.formGroup.valid) {
      this.serverError = '';
      this.userService.login(postModel)
    .subscribe(
      (res: any) => {
        if (res.succeeded) {
          if (this.returnUrl) {
            this.router.navigate([this.returnUrl]);
          } else {
            this.router.navigate(['/membersportal']);
          }

        } else {
          this.serverError = res;
        }
      },
      err => {
        this.serverError = 'Invalid username or password.';
      }
    );
    }
  }

}
