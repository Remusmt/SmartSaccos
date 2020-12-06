import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { CurrentUser } from '../shared/models/current-user';
import { Member } from '../shared/models/member';
import { AuthenticationService } from '../shared/services/authentication.service';
import { ConstantsService } from '../shared/services/constants.service';

export interface KycFormModel {
  id: number;
  surname: string;
  otherNames: string;
  gender: number;
  maritalStatus: number;
  phoneNumber: string;
  indentificationNo: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  currentUser: CurrentUser|any;

  kycForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    surname: new FormControl(''),
    otherNames: new FormControl('', Validators.required),
    gender: new FormControl(0, Validators.required),
    maritalStatus: new FormControl(0),
    phoneNumber: new FormControl('', Validators.required),
    indentificationNo: new FormControl('', Validators.required),
    email: new FormControl({value: '', disabled: true},
    [
      Validators.required,
      Validators.email
    ]),
  });
  baseurl = '';
  private emptyMember = {
    id: 0, memberNumber: '', surname: '', otherNames: '',
    gender:  0, maritalStatus:  0, dateOfBirth: undefined,
    dateJoined: new Date(), phoneNumber: '', email: '',
    memberStatus:  0, applicationUserId:  0, indentificationNo: '',
    passportPhotoId:  0, idFrontAttachmentId:  0, idBackAttachmentId:  0,
    MemberAttachments: []
  };

  private currentMemberSubject = new BehaviorSubject<Member>(this.emptyMember);
  public currentMember = this.currentMemberSubject.asObservable();

  constructor(
    private http: HttpClient,
    private constants: ConstantsService,
    private authenticationService: AuthenticationService) {

    this.baseurl = `${this.constants.baseUrl}Members/`;
    this.authenticationService.currentUser.subscribe(
      res => this.currentUser = res
    );
    if (this.currentUser.status > 0) { // member as done some form of kyc
      const member = this.refreshMember(this.currentUser.memberId);
      if (!this.isMember(member)) { return; } // value not a valid member
      // populate set member
      this.currentMemberSubject.next(member);
    } else { // Registered user no kyc entered
      this.populateForm({
        id: this.currentUser.memberId,
        surname:  this.currentUser.surname,
        otherNames: this.currentUser.otherNames,
        gender: 0,
        maritalStatus: 0,
        phoneNumber: this.currentUser.phoneNumber,
        indentificationNo: '',
        email: this.currentUser.email
      });
    }
  }

  private populateForm(value: KycFormModel): void {
    this.kycForm.setValue({
      id: value.id,
      surname:  value.surname,
      otherNames: value.otherNames,
      gender: value.gender,
      maritalStatus: value.maritalStatus,
      phoneNumber: value.phoneNumber,
      indentificationNo: value.indentificationNo,
      email: value.email
    });
  }

  // verify returned value is of type member
  private isMember(value: Member | any): value is Member {
    return (value as Member).id !== undefined;
  }

  refreshMember(id: number, detailed = false): Member {
    if (detailed) {
      this.getDetailedMember(id).subscribe(
        res => {
          return res;
        }
      );
    } else {
      this.getMember(id).subscribe(
        res => {
          this.populateForm({
            id: res.id,
            surname:  res.surname,
            otherNames: res.otherNames,
            gender: res.gender,
            maritalStatus: res.maritalStatus,
            phoneNumber: res.phoneNumber,
            indentificationNo: res.indentificationNo,
            email: res.email
          });
          return res;
        }
      );
    }
    return this.emptyMember;
  }

  getMember(id: number): Observable<Member> {
    return this.http.get<Member>(`${this.baseurl}GetMember/${id}`);
  }

  getDetailedMember(id: number): Observable<Member> {
    return this.http.get<Member>(`${this.baseurl}GetDetailedMember/${id}`).
    pipe(
      map(
        res => {
          if (this.isMember(res)) {
            this.currentMemberSubject.next(res);
          }
          return res;
        }
      )
    );
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

  onCompleteKyc(): Observable<Member> {
    return this.http.get<Member>(`${this.baseurl}CompleteKyc`)
    .pipe(
      map(
        res => {
          if (this.isMember(res)) {
            this.currentUser.status = res.memberStatus;
            this.currentUser.weKnowCustomer = res.memberStatus > 1;
            this.authenticationService.setUser(this.currentUser);
          }
          return res;
        }
      )
    );
  }

}
