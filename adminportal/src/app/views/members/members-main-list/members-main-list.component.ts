import { Component } from '@angular/core';
import {Location} from '@angular/common';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-members-main-list',
  templateUrl: './members-main-list.component.html',
  styleUrls: ['./members-main-list.component.css']
})
export class MembersMainListComponent {

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
