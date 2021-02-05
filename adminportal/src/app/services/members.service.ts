import { Member } from './../shared/models/member';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { CurrentUser } from '../shared/models/current-user';
import { AuthenticationService } from '../shared/services/authentication.service';
import { ConstantsService } from '../shared/services/constants.service';
import { ApprovalModel } from '../models/approval-modal';
import { Attachment } from '../shared/models/attachment';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  currentUser: CurrentUser|any;
  private wasFetched = false;

  approvalForm: FormGroup = new FormGroup({
    memberId: new FormControl(null),
    messageToMember: new FormControl(''),
    comments: new FormControl(''),
    actionType: new FormControl(0),
  });
  baseurl = '';
  controller = 'Admin';

  memberStatuses = [
    {value: 0, description: 'Pending KYC'},
    {value: 1, description: 'Pending KYC'},
    {value: 2, description: 'Pending KYC'},
    {value: 3, description: 'Pending Approval'},
    {value: 4, description: 'Membership Fee Pending'},
    {value: 5, description: 'Active'},
    {value: 6, description: 'On Hold'},
    {value: 7, description: 'Rejected'},
    {value: 8, description: 'Terminated'}
  ];
  maritalStatuses = [
    {value: 0 , description: 'Single'},
    {value: 1 , description: 'Married'},
    {value: 2 , description: 'Divorced'},
    {value: 3 , description: 'widowed'}
  ];
  occupationTypes = [
    {value: 0, description: 'Employed'},
    {value: 1, description: 'Self Employed'},
    {value: 2, description: 'Employed & In Bussiness'}
  ];

  memberSource = [
    {value: 0, description: 'Member'},
    {value: 1, description: 'Friend'},
    {value: 2, description: 'Website'},
    {value: 3, description: 'Advertisement'}
  ];

  private selectedMemberSubject = new BehaviorSubject<Member>(new Member());
  public selectedMember = this.selectedMemberSubject.asObservable();

  private dataSourceSubject = new BehaviorSubject<Member[]>([]);
  public dataSource = this.dataSourceSubject.asObservable();

  constructor(
    private http: HttpClient,
    private constants: ConstantsService,
    private authenticationService: AuthenticationService) {

    this.baseurl = `${this.constants.baseUrl}Admin/`;
    this.authenticationService.currentUser.subscribe(
      res => this.currentUser = res
    );
    this.onGetMembers();
  }

  initApprovalForm(memberId: number, action: number): void {
    this.approvalForm.setValue(
      {
        memberId,
        messageToMember: '',
        comments: '',
        actionType: action
      }
    );
  }

  onGetMembers(): void {
    if (!this.wasFetched) {
      const requestUrl = `${this.constants.baseUrl}${this.controller}/GetMembers`;
      this.http.get<Member[]>(requestUrl)
      .subscribe(
        res => {
          this.dataSourceSubject.next(res);
          if (res.length > 0) {
            this.setSelected(res[0]);
          }
          this.wasFetched = true;
        }
      );
    }
  }

  public setSelected(member: Member): void {
    this.selectedMemberSubject.next(member);
  }

  // verify returned value is of type member
  public isMember(value: Member | any): value is Member {
    return (value as Member).id !== undefined;
  }

  kycDetails(model: Member): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}KycDetails`, model)
    .pipe(
      map(
        res => {
          if (this.isMember(res)) {
            this.currentUser.status = res.memberStatus;
            this.authenticationService.setUser(this.currentUser);
          }
          return res;
        }
      )
    );
  }

  uploadImage(file: File|any, kycDocType: number): Observable<Member> {
    const uploadData = new FormData();
    uploadData.append('kycDoc', file, file.name);
    uploadData.append('kycDocType', kycDocType.toString());
    uploadData.append('Member', this.selectedMemberSubject.value.id.toString());
    return this.http.post<Member>(`${this.baseurl}KycDocs`, uploadData)
    .pipe(
      map(
        res => {
          if (this.isMember(res)) {
            this.selectedMemberSubject.next(res);
          }
          return res;
        }
      )
    );
  }

  approveRegistration(model: ApprovalModel): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}ApproveMember`, model)
    .pipe(
      map(
        res => {
          // update datasource
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res.id);
          if (index >= 0) {
            this.dataSourceSubject.value[index] = res;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          this.selectedMemberSubject.next(res);
          return res;
        }
      )
    );
  }

  putMemberOnHold(model: ApprovalModel): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}PutMemberOnHold`, model)
    .pipe(
      map(
        res => {
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res.id);
          if (index >= 0) {
            this.dataSourceSubject.value[index] = res;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          this.selectedMemberSubject.next(res);
          return res;
        }
      )
    );
  }

  rejectMember(model: ApprovalModel): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}RejectMember`, model)
    .pipe(
      map(
        res => {
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res.id);
          if (index >= 0) {
            this.dataSourceSubject.value[index] = res;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          this.selectedMemberSubject.next(res);
          return res;
        }
      )
    );
  }

  getMaritalStatusDescription(status: number): string|any {
    return this.maritalStatuses.find(e => e.value)?.description;
  }

  getStatusDesription(memberStatus: number): string|any {
    return this.memberStatuses.find(e => e.value === memberStatus)?.description;
  }

  getOccupationDescription(value: number): string|any {
    return this.occupationTypes.find(e => e.value === value)?.description;
  }

  getMemberSourceDescription(value: number): string|any {
    return this.memberSource.find(e => e.value === value)?.description;
  }

  getGenderDescription(gender: number): string {
    return gender > 0 ? 'Male' : 'Female';
  }

  intial = () => {
    let initials = '';
    if (this.selectedMemberSubject.value.otherNames.length > 0) {
      initials = this.selectedMemberSubject.value.otherNames.substr(0, 1);
    }
    if (this.selectedMemberSubject.value.surname.length > 0) {
      initials = `${initials} ${this.selectedMemberSubject.value.surname.substr(0, 1)}`;
    }
    return initials;
  }

  avatorName = () => {
    const avatorName = this.getAttachmentName(this.selectedMemberSubject.value.passportPhotoId);
    return avatorName ? `${this.constants.resourceUrl}${avatorName}` : '';
  }

  idFrontName = () => {
    const rName = this.getAttachmentName(this.selectedMemberSubject.value.idFrontAttachmentId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  idBackName = () => {
    const rName = this.getAttachmentName(this.selectedMemberSubject.value.idBackAttachmentId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  passportCopyName = () => {
    const rName = this.getAttachmentName(this.selectedMemberSubject.value.passportCopyId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  signatureName = () => {
    const rName = this.getAttachmentName(this.selectedMemberSubject.value.signatureId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  private getAttachmentName(id: number): string {
    if (id === 0) {
      return '';
    }
    let name = '';
    if (this.selectedMemberSubject.value.memberAttachments.length > 0) {
      const attachment = this.getAttachment(id);
      if (attachment !== undefined) {
        name = `${attachment.systemFileName}${attachment.extension}`;
      }
    }
    return name;
  }

  private getAttachment(id: number): Attachment|undefined {
    return this.selectedMemberSubject.value.memberAttachments
    .find((e: any) => e.id === id)?.attachment;
  }

}
