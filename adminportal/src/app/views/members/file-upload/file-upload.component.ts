import { Component, Input } from '@angular/core';
import { MembersService } from 'src/app/services/members.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent {

  files: File[] = [];
  rejectedFiles: File[] = [];

  @Input() attachmentType = '0';

  constructor(
    private service: MembersService,
    private notification: NotificationService
  ) { }

  onSelect(event: any): void {
    this.files.splice(0, 1);
    this.rejectedFiles.splice(0, 1);
    this.files.push(...event.addedFiles);
    this.rejectedFiles.push(...event.rejectedFiles);
    if (this.files.length > 0) {
      this.service.uploadImage(this.files[0], Number.parseInt(this.attachmentType, 10))
      .subscribe(
        () => {
          this.notification.success('Uploaded successfully');
        },
        (err: any) => {
          this.notification.warn(err);
        }
      );
    }
  }

  onRemove(event: File): void {
    this.files.splice(this.files.indexOf(event), 1);
  }

}
