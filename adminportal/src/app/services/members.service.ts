import { Member } from './../shared/models/member';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { CurrentUser } from '../shared/models/current-user';
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
  private wasFetched = false;

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
  controller = 'Admin';

  private currentMemberSubject = new BehaviorSubject<Member>(new Member());
  public currentMember = this.currentMemberSubject.asObservable();

  private dataSourceSubject = new BehaviorSubject<Member[]>([]);
  public dataSource = this.dataSourceSubject.asObservable();

  constructor(
    private http: HttpClient,
    private constants: ConstantsService,
    private authenticationService: AuthenticationService) {

    this.baseurl = `${this.constants.baseUrl}Members/`;
    this.authenticationService.currentUser.subscribe(
      res => this.currentUser = res
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
    this.currentMemberSubject.next(member);
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
            this.currentUser.weKnowCustomer = res.memberStatus > 2;
            this.authenticationService.setUser(this.currentUser);
          }
          return res;
        }
      )
    );
  }

}
