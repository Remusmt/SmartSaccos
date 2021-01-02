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
  nokIsMinor = false;
  changingImage = 0;

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

  getStatusDesription(memberStatus: number): string {
    switch (memberStatus) {
      case 0:
      case 1:
      case 2:
        return 'Pending KYC';
      case 3:
      case 6:
        return 'Pending Approval';
      case 4:
        return 'Pay Membership Fee';
      case 5:
        return 'Active';
      case 7:
        return 'Rejected';
      case 8:
        return 'Terminated';
      default:
        return 'Unknown';
    }
  }

  getGenderDescription(gender: number): string {
    return gender > 0 ? 'Male' : 'Female';
  }

  onMinorChange(): void {
    this.nokIsMinor = !this.nokIsMinor;
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

  changeImage(imgType: number): void {

  }

}
