import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent implements OnInit, OnDestroy {

  subscription: Subscription = new Subscription();
  constructor(
    private router: Router,
    private route: ActivatedRoute){}

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    this.subscription.add(
      this.route.queryParams
      .subscribe(
        res => {
          if (res.returnUrl) {
            this.router.navigate([res.returnUrl]);
          }
        }
      )
    );
  }
}
