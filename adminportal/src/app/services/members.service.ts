import { Member } from './../shared/models/member';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
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
          this.wasFetched = true;
        }
      );
    }
  }

  public setSelected(member: Member): void {
    this.selectedMemberSubject.next(member);
  }

  // verify returned value is of type member
  private isMember(value: Member | any): value is Member {
    return (value as Member).id !== undefined;
  }

  getDetailedMember(id: number): Observable<Member> {
    return this.http.get<Member>(`${this.baseurl}GetDetailedMember/${id}`).
    pipe(
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

  uploadImage(file: File|any, kycDocType: number): any {
    const uploadData = new FormData();
    uploadData.append('kycDoc', file, file.name);
    uploadData.append('kycDocType', kycDocType.toString());
    this.http.post(`${this.baseurl}KycDocs`, uploadData, {
      reportProgress: true,
      observe: 'events'
    })
      .subscribe(_ => {
        // console.log(event); // handle event here
      });
  }

  approveRegistration(model: ApprovalModel): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}ApproveMember`, model)
    .pipe(
      map(
        res => {
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
          this.selectedMemberSubject.next(res);
          return res;
        }
      )
    );
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
