import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { CurrentUser } from '../shared/models/current-user';
import { Member } from '../shared/models/member';
import { AuthenticationService } from '../shared/services/authentication.service';
import { ConstantsService } from '../shared/services/constants.service';

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

  private currentMemberSubject = new BehaviorSubject<Member>({
    id: 0, memberNumber: '', surname: '', otherNames: '',
    gender:  0, maritalStatus:  0, dateOfBirth: undefined,
    dateJoined: new Date(), phoneNumber: '', email: '',
    memberStatus:  0, applicationUserId:  0, indentificationNo: '',
    passportPhotoId:  0, idFrontAttachmentId:  0, idBackAttachmentId:  0,
    MemberAttachments: []
  });
  public currentMember = this.currentMemberSubject.asObservable();

  constructor(
    private http: HttpClient,
    private constants: ConstantsService,
    private authenticationService: AuthenticationService) {

    this.baseurl = `${this.constants.baseUrl}Members/`;
    this.authenticationService.currentUser.subscribe(
      res => this.currentUser = res
    );

    this.kycForm.setValue({
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

  KycDetails(model: Member): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}KycDetails`, model);
  }

}
