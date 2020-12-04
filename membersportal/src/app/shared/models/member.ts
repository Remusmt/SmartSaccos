import { Attachment } from './attachment';

export interface Member {
  id: number;
  memberNumber: string;
  surname: string;
  otherNames: string;
  gender: number;
  maritalStatus: number;
  dateOfBirth?: Date;
  dateJoined: Date;
  phoneNumber: string;
  email: string;
  memberStatus: number;
  applicationUserId: number;
  indentificationNo: string;
  passportPhotoId: number;
  idFrontAttachmentId: number;
  idBackAttachmentId: number;
  MemberAttachments: Array<MemberAttachment>;
}

export interface MemberAttachment {
  id: number;
  memberId: number;
  attachmentId: number;
  attachment: Attachment;
}

