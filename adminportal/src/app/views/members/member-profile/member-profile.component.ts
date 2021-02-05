import { Observable, Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MembersService } from 'src/app/services/members.service';
import { Member } from 'src/app/shared/models/member';
import { RegistrationActionComponent } from '../../registration-action/registration-action.component';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import {Location} from '@angular/common';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-member-profile',
  templateUrl: './member-profile.component.html',
  styleUrls: ['./member-profile.component.css']
})
export class MemberProfileComponent implements OnInit, OnDestroy {

  selectedMember: Member = new Member();
  subscriptions: Subscription = new Subscription();

  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  isMobile = false;

  constructor(
    private dialog: MatDialog,
    private location: Location,
    public service: MembersService,
    private notification: NotificationService,
    private breakpointObserver: BreakpointObserver
  ) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.service.selectedMember.subscribe(
        res => {
          this.selectedMember = res;
        }
      )
    );
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onApprove(): void {
    if (this.selectedMember === undefined) {
      return;
    }
    this.service.initApprovalForm(this.selectedMember.id, 0);
    this.dialog.open(RegistrationActionComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onReject(): void {
    if (this.selectedMember === undefined) {
      return;
    }
    this.service.initApprovalForm(this.selectedMember.id, 2);
    this.dialog.open(RegistrationActionComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onUpdate(): void {
        this.subscriptions.add(
          this.service.kycDetails(this.selectedMember).subscribe(
            _ => {
              this.notification.success('Saved Successfully');
            },
            err => {
              this.notification.warn(err);
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

  goBack(): void {
    this.location.back();
  }
}
