import { Member } from 'src/app/shared/models/member';
import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';

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
  nokIsMinor = false;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.Handset,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private router: Router,
    public service: MembersService,
    private notification: NotificationService,
    private breakpointObserver: BreakpointObserver,
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

  onMinorChange(): void {
    this.nokIsMinor = !this.nokIsMinor;
  }

  onSaveAndNext(): void {
    if (this.service.kycForm.valid) {
      this.subscription.add(
        this.service.kycPost(this.service.kycForm.value).subscribe(
          _ => {
            this.notification.success('Completed successfully');
            this.router.navigate(['/membersportal']);
          },
          err => {
            console.log(err);
            this.notification.warn('An error occured');
          }
        )
      );
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
