import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { CurrentUser } from '../shared/models/current-user';
import { Member, MemberAddress } from '../shared/models/member';
import { AuthenticationService } from '../shared/services/authentication.service';
import { ConstantsService } from '../shared/services/constants.service';
import { Attachment } from '../shared/models/attachment';

export interface KycFormModel {
  id: number;
  surname: string;
  otherNames: string;
  gender: number;
  maritalStatus: number;
  phoneNumber: string;
  indentificationNo: string;
  email: string;
  learntAboutUs: number;
  nearestTown: string;
  nextOfKin: string;
  nokContacts: string;
  nokIsMinor: boolean;
  nokRelation: string;
  occupation: string;
  occupationType: number;
  title: string;
  dateOfBirth?: Date;
  homeAddress: MemberAddress;
  permanentAddress: MemberAddress;
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  currentUser: CurrentUser|any;
  private address = {id : 0, village: '', location: '', district: '', county: '', country: ''};

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
    learntAboutUs: new FormControl(0),
    nearestTown: new FormControl(''),
    nextOfKin: new FormControl(''),
    nokContacts: new FormControl(''),
    nokIsMinor: new FormControl(''),
    nokRelation: new FormControl(''),
    occupation: new FormControl(''),
    occupationType: new FormControl(0),
    title: new FormControl(''),
    dateOfBirth: new FormControl(new Date((new Date().getTime()))),
    homeAddress: new FormGroup(
      {
        village: new FormControl(''),
        location: new FormControl(''),
        district: new FormControl(''),
        county: new FormControl(''),
        country: new FormControl(''),
      }
    ),
    permanentAddress: new FormGroup(
      {
        village: new FormControl(''),
        location: new FormControl(''),
        district: new FormControl(''),
        county: new FormControl(''),
        country: new FormControl(''),
      }
    )
  });
  baseurl = '';

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

  private currentMemberSubject = new BehaviorSubject<Member>(new Member());
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
      email: value.email,
      learntAboutUs: value.learntAboutUs,
      nearestTown: value.nearestTown,
      nextOfKin: value.nextOfKin,
      nokContacts: value.nokContacts,
      nokIsMinor: value.nokIsMinor,
      nokRelation: value.nokRelation,
      occupation: value.occupation,
      occupationType: value.occupationType,
      title: value.title,
      dateOfBirth: value.dateOfBirth,
      homeAddress: {
        village: value.homeAddress?.village,
        location: value.homeAddress?.location,
        district: value.homeAddress?.district,
        county: value.homeAddress?.county,
        country: value.homeAddress?.country
      },
      permanentAddress: {
        village: value.permanentAddress?.village,
        location: value.permanentAddress?.location,
        district: value.permanentAddress?.district,
        county: value.permanentAddress?.county,
        country: value.permanentAddress?.country
      }
    });
  }

  // verify returned value is of type member
  private isMember(value: Member | any): value is Member {
    return (value as Member).id !== undefined;
  }

  refreshMember(id: number, detailed = true): Member {
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
            email: res.email,
            learntAboutUs: res.learntAboutUs,
            nearestTown: res.nearestTown,
            nextOfKin: res.nextOfKin,
            nokContacts: res.nokContacts,
            nokIsMinor: res.nokIsMinor,
            nokRelation: res.nokRelation,
            occupation: res.occupation,
            occupationType: res.occupationType,
            title: res.title,
            dateOfBirth: res.dateOfBirth,
            homeAddress: res.homeAddress ? res.homeAddress : this.address,
            permanentAddress: res.permanentAddress ? res.permanentAddress : this.address
          });
          return res;
        }
      );
    }
    return new Member();
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
            this.populateForm(
              {
                id: res.id,
                surname:  res.surname,
                otherNames: res.otherNames,
                gender: res.gender,
                maritalStatus: res.maritalStatus,
                phoneNumber: res.phoneNumber,
                indentificationNo: res.indentificationNo,
                email: res.email,
                learntAboutUs: res.learntAboutUs,
                nearestTown: res.nearestTown,
                nextOfKin: res.nextOfKin,
                nokContacts: res.nokContacts,
                nokIsMinor: res.nokIsMinor,
                nokRelation: res.nokRelation,
                occupation: res.occupation,
                occupationType: res.occupationType,
                title: res.title,
                dateOfBirth: res.dateOfBirth,
                homeAddress: res.homeAddress ? res.homeAddress : this.address,
                permanentAddress: res.permanentAddress ? res.permanentAddress : this.address
              }
            );
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

  uploadImage(file: File|any, kycDocType: number): Observable<Member> {
    const uploadData = new FormData();
    uploadData.append('kycDoc', file, file.name);
    uploadData.append('kycDocType', kycDocType.toString());
    return this.http.post<Member>(`${this.baseurl}KycDocs`, uploadData)
    .pipe(
      map(
        res => {
          if (this.isMember(res)) {
            this.currentUser.status = res.memberStatus;
            this.currentMemberSubject.next(res);
          }
          return res;
        }
      )
    );
  }

  kycPost(model: Member): Observable<Member> {
    return this.http.post<Member>(`${this.baseurl}KycPost`, model)
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


  intial = () => {
    let initials = '';
    if (this.currentUser.otherNames.length > 0) {
      initials = this.currentUser.otherNames.substr(0, 1);
    }
    if (this.currentUser.surname.length > 0) {
      initials = `${initials} ${this.currentUser.surname.substr(0, 1)}`;
    }
    return initials;
  }

  avatorName = () => {
    const avatorName = this.getAttachmentName(this.currentMemberSubject.value.passportPhotoId);
    return avatorName ? `${this.constants.resourceUrl}${avatorName}` : '';
  }

  idFrontName = () => {
    const rName = this.getAttachmentName(this.currentMemberSubject.value.idFrontAttachmentId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  idBackName = () => {
    const rName = this.getAttachmentName(this.currentMemberSubject.value.idBackAttachmentId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  passportCopyName = () => {
    const rName = this.getAttachmentName(this.currentMemberSubject.value.passportCopyId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  signatureName = () => {
    const rName = this.getAttachmentName(this.currentMemberSubject.value.signatureId);
    return rName ? `${this.constants.resourceUrl}${rName}` : '';
  }

  private getAttachmentName(id: number): string {
    if (id === 0) {
      return '';
    }
    let name = '';
    if (this.currentMemberSubject.value.memberAttachments.length > 0) {
      const attachment = this.getAttachment(id);
      if (attachment !== undefined) {
        name = `${attachment.systemFileName}${attachment.extension}`;
      }
    }
    return name;
  }

  private getAttachment(id: number): Attachment|undefined {
    return this.currentMemberSubject.value.memberAttachments
    .find((e: any) => e.id === id)?.attachment;
  }
}
