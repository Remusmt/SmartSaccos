import { NotificationService } from './../../shared/services/notification.service';
import { RegisterService } from './../../services/register.service';
import { ConstantsService } from './../../shared/services/constants.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmedValidator } from 'src/app/shared/helpers/custom-password-validator';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {

  registerForm: FormGroup|any;
  serverError = '';
  subscription: Subscription  = new Subscription();
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private constants: ConstantsService,
    private registerService: RegisterService,
    private notificationService: NotificationService) {

    this.registerForm = this.fb.group({
      email: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      surname: new FormControl(''),
      otherNames: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6)
      ]),
      confirmPassword: new FormControl('', [
        Validators.required
      ]),
      phoneNumber: new FormControl(''),
      companyName: new FormControl(this.constants.company),
      countryId: new FormControl(this.constants.countryId)
    }, {
      validator: ConfirmedValidator('password', 'confirmPassword')
    });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
  }

  getPasswordErrors(): string {
    if (this.registerForm.get('password').hasError('required')) {
      return 'Password is required';
    }
    if (this.registerForm.get('password').errors.minlength) {
      return 'Password must be at least 6 characters long.';
    }
    return '';
  }

  getConfirmPasswordErrors(): string {
    if (this.registerForm.get('confirmPassword').hasError('required')) {
      return 'Confrim password required';
    }
    if (this.registerForm.controls.confirmPassword.errors.confirmedValidator) {
      return 'Password and Confirm Password must be match';
    }
    return '';
  }

  submitForm(): void {
    if (this.registerForm.valid) {
      this.subscription.add(
        this.registerService.Register(this.registerForm.value).subscribe(
          res => {
            if (res.id > 0) {
              this.notificationService.success(`${res.otherNames} ${res.surname} account created successfuly`);
              this.router.navigate(['/home']);
            } else {
              this.notificationService.warn('An error occured please try again');
            }
          },
          err => {
            this.notificationService.warn(err);
          }
        )
      );
    }

  }

}
