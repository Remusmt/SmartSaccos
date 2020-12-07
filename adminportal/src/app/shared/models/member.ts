import { Attachment } from './attachment';

export class Member {
  id = 0;
  memberNumber = '';
  surname = '';
  otherNames = '';
  gender = 0;
  maritalStatus = 0;
  phoneNumber = '';
  email = '';
  memberStatus = 0;
  applicationUserId = 0;
  indentificationNo = '';
  passportPhotoId = 0;
  idFrontAttachmentId = 0;
  idBackAttachmentId = 0;
  dateOfBirth?: Date;
  dateJoined: Date|any;
  memberAttachments: Array<MemberAttachment> = [];
  // constructor() {
  //   this.id = 0;
  //   this.memberNumber = '';
  //   this.surname = '';
  //   this.otherNames = '';
  //   this.gender = 0;
  //   this.maritalStatus = 0;
  //   this.phoneNumber = '';
  //   this.email = '';
  //   this.memberStatus = 0;
  //   this.applicationUserId = 0;
  //   this.indentificationNo = '';
  //   this.passportPhotoId = 0;
  //   this.idFrontAttachmentId = 0;
  //   this.idBackAttachmentId = 0;
  //   this.memberAttachments = [];
  // }

  intial = () => {
    let initials = '';
    if (this.otherNames.length > 0) {
      initials = this.otherNames.substr(0, 1);
    }
    if (this.surname.length > 0) {
      initials = `${initials} ${this.surname.substr(0, 1)}`;
    }
    return initials;
  }

  public avatorName = () => {
    return this.getAttachmentName(this.passportPhotoId);
  }

  public get idFrontName(): string {
    return this.getAttachmentName(this.idFrontAttachmentId);
  }

  public get idBackName(): string {
    return this.getAttachmentName(this.idBackAttachmentId);
  }

  private getAttachmentName(id: number): string {
    if (id === 0) {
      return '';
    }
    let name = '';
    if (this.memberAttachments.length > 0) {
      const attachment = this.getAttachment(this.passportPhotoId);
      if (attachment !== undefined) {
        name = `${attachment.systemFileName}.${attachment.extension}`;
      }
    }
    return name;
  }

  private getAttachment(id: number): Attachment|undefined {
    return this.memberAttachments.find(e => e.id === id)?.attachment;
  }

}

export interface MemberAttachment {
  id: number;
  memberId: number;
  attachmentId: number;
  attachment: Attachment;
}

