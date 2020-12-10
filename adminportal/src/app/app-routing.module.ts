import { MembersMainListComponent } from './views/members/members-main-list/members-main-list.component';
import { MembersMainComponent } from './views/members/members-main/members-main.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './shared/services/auth.guard';
import { HomeComponent } from './views/home/home.component';
import { LoginComponent } from './views/login/login.component';
import { MemberProfileComponent } from './views/members/member-profile/member-profile.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'app', component: HomeComponent, canActivate: [AuthGuard],
  children: [
    {path: '', component: MembersMainListComponent},
    {path: 'members', component: MembersMainComponent},
    {path: 'memberslist', component: MembersMainListComponent},
    {path: 'memberprofile', component: MemberProfileComponent}
  ]},
  {path: '', redirectTo: '/app', pathMatch: 'full'},
  {path: '**', component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
