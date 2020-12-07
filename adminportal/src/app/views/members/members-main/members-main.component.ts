import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import {Location} from '@angular/common';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-members-main',
  templateUrl: './members-main.component.html',
  styleUrls: ['./members-main.component.css']
})
export class MembersMainComponent {

  isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private breakpointObserver: BreakpointObserver,
    private location: Location
  ) { }

  goBack(): void {
    this.location.back();
  }
}
