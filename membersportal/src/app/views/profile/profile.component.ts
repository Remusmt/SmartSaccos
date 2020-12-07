import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MembersService } from 'src/app/services/members.service';
import { Member } from 'src/app/shared/models/member';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

  currentMember: Member|any;
  subscription: Subscription = new Subscription();
  constructor(
    public service: MembersService) {}

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

  getGenderDescription(gender: number): string {
    return gender === 0 ? 'Female' : 'Male';
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
}
