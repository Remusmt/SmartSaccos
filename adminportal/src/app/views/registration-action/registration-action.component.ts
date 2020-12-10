import { NotificationService } from './../../shared/services/notification.service';
import { Subscription } from 'rxjs';
import { MembersService } from './../../services/members.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Member } from 'src/app/shared/models/member';

@Component({
  selector: 'app-registration-action',
  templateUrl: './registration-action.component.html',
  styleUrls: ['./registration-action.component.css']
})
export class RegistrationActionComponent implements OnInit, OnDestroy {

  saving = false;
  selectedMember: Member = new Member();
  subscriptions: Subscription = new Subscription();

  constructor(
    public service: MembersService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<RegistrationActionComponent>
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.service.selectedMember.subscribe(
        res => {
          this.selectedMember = res;
        }
      )
    );
  }

  onSubmit(): void {
    switch (this.service.approvalForm.controls['actionType'].value) {
      case 0:
        this.subscriptions.add(
          this.service.approveRegistration(this.service.approvalForm.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Approved successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              this.notificationService.warn(err);
              this.saving = false;
            }
        ));
        break;
      case 1:
        this.subscriptions.add(
          this.service.putMemberOnHold(this.service.approvalForm.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Updated successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              this.notificationService.warn(err);
              this.saving = false;
            }
        ));
        break;
       case 2:
        this.subscriptions.add(
          this.service.rejectMember(this.service.approvalForm.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Rejected successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              this.notificationService.warn(err);
              this.saving = false;
            }
        ));
        break;
    }
  }

  onClose(): void {
    this.service.approvalForm.reset();
    this.dialogRef.close();
  }

}
