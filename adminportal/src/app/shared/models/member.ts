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

