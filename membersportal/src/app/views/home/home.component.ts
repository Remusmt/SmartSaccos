import { Subscription } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  currentUser: CurrentUser|any;
  subscription: Subscription = new Subscription();

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) {
      this.subscription.add(
        this.authenticationService.currentUser.subscribe(
          res => this.currentUser = res
        )
      );
    }

  ngOnInit(): void {
    if (!this.currentUser.weKnowCustomer) {
      this.router.navigate(['/kyc']);
    }
  }

}
