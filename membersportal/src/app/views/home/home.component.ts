import { ConstantsService } from './../../shared/services/constants.service';
import { MembersService } from './../../services/members.service';
import { Observable, Subscription } from 'rxjs';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import { MatSidenav } from '@angular/material/sidenav';
import { Member } from 'src/app/shared/models/member';
import { Attachment } from 'src/app/shared/models/attachment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {

  currentUser: CurrentUser|any;
  currentMember = new  Member();
  subscription: Subscription = new Subscription();
  isHandset$: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.Handset,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
    isMobile = false;
  constructor(
    private router: Router,
    private service: MembersService,
    private constants: ConstantsService,
    private breakpointObserver: BreakpointObserver,
    private authenticationService: AuthenticationService) {
      this.subscription.add(
        this.authenticationService.currentUser.subscribe(
          res => this.currentUser = res
        )
      );
    }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    this.subscription.add(
      this.isHandset$.subscribe(
        res => {
          this.isMobile = res;
        }
      )
    );

    this.service.getDetailedMember(this.currentUser.memberId)
    .subscribe(
      res => {
        this.currentMember = res;
      }
    );
  }

  toggleSideNav(drawer: MatSidenav): void {
    if (this.isMobile) {
      drawer.toggle();
    }
  }

  logout(): void {
    this.authenticationService.logout(false);
    this.router.navigate(['/home']);
  }

  viewProfile(): void {
    this.router.navigate(['/kyc']);
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
    const avatorName = this.getAttachmentName(this.currentMember.passportPhotoId);
    return avatorName ? `${this.constants.resourceUrl}${avatorName}` : '';
  }

  idFrontName = () => {
    return this.getAttachmentName(this.currentMember.idFrontAttachmentId);
  }

  idBackName = () => {
    return this.getAttachmentName(this.currentMember.idBackAttachmentId);
  }

  private getAttachmentName(id: number): string {
    if (id === 0) {
      return '';
    }
    let name = '';
    if (this.currentMember.memberAttachments.length > 0) {
      const attachment = this.getAttachment(this.currentMember.passportPhotoId);
      if (attachment !== undefined) {
        name = `${attachment.systemFileName}${attachment.extension}`;
      }
    }
    return name;
  }

  private getAttachment(id: number): Attachment|undefined {
    return this.currentMember.memberAttachments
    .find((e: any) => e.id === id)?.attachment;
  }



}
