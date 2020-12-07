import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MembersService } from 'src/app/services/members.service';
import { Member } from 'src/app/shared/models/member';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit, OnDestroy {

  currentMember: Member|any;
  subscription: Subscription = new Subscription();
  constructor(
    private service: MembersService) {}

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    this.subscription.add(
      this.service.currentMember.subscribe(
        res => {
          this.currentMember = res;
        }
      )
    );
  }

}
