import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  @Input() ShowLinks = false;

  isHandset: Observable<boolean> = this.breakpointObserver
  .observe([
    Breakpoints.Medium,
    Breakpoints.Large,
    Breakpoints.XLarge
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  constructor(private breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {
  }

}
