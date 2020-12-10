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
  passportCopyId = 0;
  homeAddressId = 0;
  learntAboutUs = 0;
  nearestTown = '';
  nextOfKin = '';
  nokContacts = '';
  nokIsMinor = false;
  nokRelation = '';
  occupation = '';
  occupationType = 0;
  permanentAddressId = 0;
  signatureId = 0;
  title = '';
  dateOfBirth?: Date;
  dateJoined: Date|any;
  homeAddress = new MemberAddress();
  permanentAddress = new MemberAddress();
  memberAttachments: Array<MemberAttachment> = [];

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

export class MemberAddress {
  id = 0;
  village = '';
  location = '';
  district = '';
  county = '';
  country = '';
}
