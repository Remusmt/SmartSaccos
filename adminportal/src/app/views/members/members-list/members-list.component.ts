import { Member } from './../../../shared/models/member';
import { MembersService } from './../../../services/members.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
export class MembersListComponent implements OnInit, OnDestroy {

  @Input() displayedStatus = 0;
  listData: MatTableDataSource<Member[]>|any;
  displayedColumns: string[] = ['memberNumber', 'otherNames', 'surname', 'phoneNumber', 'memberStatus', 'actions'];
  displayedMobileColumns: string[] = ['memberNumber', 'otherNames', 'surname', 'actions'];
  @ViewChild(MatSort) sort: MatSort|any;
  @ViewChild(MatPaginator) paginator: MatPaginator|any;

  isMobile = false;
  private selectedMemberId = 0;
  private subscriptions: Subscription = new Subscription();

  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private router: Router,
    private service: MembersService,
    private breakpointObserver: BreakpointObserver
  ) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));

    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          switch (this.displayedStatus) {
            case 3: // Pending approval
              this.listData = new MatTableDataSource(
                res.filter(e => e.memberStatus <= 3)
                );
              break;
            case 4: // Pending membership payment
              this.listData = new MatTableDataSource(
                res.filter(e => e.memberStatus === 4 || e.memberStatus === 5)
                );
              break;
            case 5: // Active
              this.listData = new MatTableDataSource(
                res.filter(e => e.memberStatus === 5)
                );
              break;
            case 6: // On hold
              this.listData = new MatTableDataSource(
                res.filter(e => e.memberStatus === 6)
                );
              break;
            case 7: // Rejected
              this.listData = new MatTableDataSource(
                res.filter(e => e.memberStatus === 7)
                );
              break;
            default:
              this.listData = new MatTableDataSource(res);
              break;
          }
          this.listData.sort = this.sort;
          this.listData.paginator = this.paginator;

          if (res.length > 0) {
            if (this.selectedMemberId > 0) {
              this.onRowClicked(res.find(e => e.id === this.selectedMemberId));
            } else {
              this.onRowClicked(res[0]);
            }
          }
        }
    ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  getColumnsToDisplay(): string[] {
    switch (this.displayedStatus) {
      case 3:
      case 7:
        if (this.isMobile) {
          return ['otherNames', 'surname', 'actions'];
        } else {
          return ['otherNames', 'surname', 'phoneNumber', 'email', 'actions'];
        }
      default:
        if (this.isMobile) {
          return ['memberNumber', 'otherNames', 'surname', 'actions'];
        } else {
          return ['memberNumber', 'otherNames', 'surname', 'phoneNumber', 'memberStatus', 'actions'];
        }
    }
  }

  isSelectedRow(id: number): boolean {
    return this.selectedMemberId === id;
  }

  onRowClicked(member: Member|any): void {
    this.selectedMemberId = member.id;
    this.service.setSelected(member);
  }

  getStatusDescription(status: number): string {
    switch (status) {
      case 0:
      case 1:
      case 2:
      case 3:
        return 'Pending';
      case 4:
        return 'Pending Membership';
      case 5:
        return 'Active';
      case 6:
        return 'On Hold';
      case 7:
        return 'Rejected';
      case 8:
        return 'Left';
      default:
        return '';
    }
  }

  onView(member: Member|any): void {
    this.onRowClicked(member);
    this.router.navigate(['/app/memberprofile']);
  }

}
