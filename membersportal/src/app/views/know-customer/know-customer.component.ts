import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-know-customer',
  templateUrl: './know-customer.component.html',
  styleUrls: ['./know-customer.component.css']
})
export class KnowCustomerComponent implements OnInit {

  currentUser: CurrentUser|any;
  subscription: Subscription = new Subscription();
  maritalStatuses = [
    {value: 0 , description: 'Single'},
    {value: 1 , description: 'Married'},
    {value: 2 , description: 'Divorced'},
    {value: 3 , description: 'widowed'}
  ];
  gender = 0;

  constructor(
    private router: Router,
    public service: MembersService,
    private authenticationService: AuthenticationService) {
      this.subscription.add(
        this.authenticationService.currentUser.subscribe(
          res => this.currentUser = res
        )
      );
    }

  ngOnInit(): void {

  }

  onGenderChanged(): void {
    this.gender = this.gender > 0 ? 0 : 1;
    this.service.kycForm.controls.gender
        .setValue(this.gender);
  }

  onSaveAndNext(stepper: any): void {
    if (this.service.kycForm.valid) {
      this.subscription.add(
        this.service.KycDetails(this.service.kycForm.value).subscribe(
          err => {
            stepper.next();
          }
        )
      );
    }
  }

}
