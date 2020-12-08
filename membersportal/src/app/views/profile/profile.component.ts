import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MembersService } from 'src/app/services/members.service';
import { Member } from 'src/app/shared/models/member';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

  currentMember: Member|any;
  subscription: Subscription = new Subscription();
  maritalStatuses = [
    {value: 0 , description: 'Single'},
    {value: 1 , description: 'Married'},
    {value: 2 , description: 'Divorced'},
    {value: 3 , description: 'widowed'}
  ];
  gender = 1;

  constructor(
    public service: MembersService,
    private notification: NotificationService) {}

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    this.subscription.add(
      this.service.currentMember.subscribe(
        res => {
          this.currentMember = res;
        }
      )
    );
  }

  onGenderChanged(): void {
    this.gender = this.gender > 0 ? 0 : 1;
    this.service.kycForm.controls.gender
        .setValue(this.gender);
  }

  getMaritalStatusDescription(status: number): string {

    switch (status) {
      case 0:
        return 'Single';
      case 1:
        return 'Married';
      case 2:
        return 'Divorced';
      case 3:
        return 'widowed';
      default:
        return 'Single';
    }
  }

  onSaveDetails(): void {
    if (this.service.kycForm.valid) {
      if (this.service.kycForm.dirty) {
        this.subscription.add(
          this.service.kycDetails(this.service.kycForm.value).subscribe(
            _ => {
              this.notification.success('Saved Successfully');
            },
            err => {
              this.notification.warn(err);
            }
          )
        );
      }
    }
  }

}
