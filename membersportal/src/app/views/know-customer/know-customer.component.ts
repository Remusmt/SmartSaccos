import { Member } from 'src/app/shared/models/member';
import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-know-customer',
  templateUrl: './know-customer.component.html',
  styleUrls: ['./know-customer.component.css']
})
export class KnowCustomerComponent implements OnInit {

  memberId = 0;
  currentMember = new  Member();
  subscription: Subscription = new Subscription();
  maritalStatuses = [
    {value: 0 , description: 'Single'},
    {value: 1 , description: 'Married'},
    {value: 2 , description: 'Divorced'},
    {value: 3 , description: 'widowed'}
  ];
  gender = 0;
  trueInfo = false;

  constructor(
    private router: Router,
    public service: MembersService,
    private notification: NotificationService,
    private authenticationService: AuthenticationService) {
      this.subscription.add(
        this.authenticationService.currentUser.subscribe(
          res => this.memberId = res.memberId
        )
      );
    }

  ngOnInit(): void {
    this.service.getDetailedMember(this.memberId).subscribe(
      res => {
        this.currentMember = res;
      }
    );
  }

  onGenderChanged(): void {
    this.gender = this.gender > 0 ? 0 : 1;
    this.service.kycForm.controls.gender
        .setValue(this.gender);
  }

  onSaveAndNext(stepper: any): void {
    if (this.service.kycForm.valid) {
      if (this.service.kycForm.dirty) {
        this.subscription.add(
          this.service.kycDetails(this.service.kycForm.value).subscribe(
            _ => {
              stepper.next();
            },
            err => {
              console.log(err);
              this.notification.warn('An error occured');
            }
          )
        );
      } else {
        stepper.next();
      }
    }
  }

  onCompleteKyc(): void {
    this.service.onCompleteKyc().
      subscribe(
        _ => {
          this.notification.success('Completed successfully');
          this.router.navigate(['/membersportal']);
        }
      );
  }




}
