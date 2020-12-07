import { Member } from './../../../shared/models/member';
import { MembersService } from './../../../services/members.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, Input, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
export class MembersListComponent implements OnInit, OnDestroy {

  @Input() filterType = 0;
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
    private dialog: MatDialog,
    private service: MembersService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));
    this.service.onGetMembers();
    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          this.listData = new MatTableDataSource(res);
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

  onView(member: Member): void {

  }

}
