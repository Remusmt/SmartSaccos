import { WelcomeComponent } from './views/welcome/welcome.component';
import { ProfileComponent } from './views/profile/profile.component';
import { KnowCustomerComponent } from './views/know-customer/know-customer.component';
import { HomeComponent } from './views/home/home.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/register/register.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LandingPageComponent } from './site/landing-page/landing-page.component';
import { AuthGuard } from './shared/services/auth.guard';

const routes: Routes = [
  {path: '', component: LandingPageComponent},
  {path: 'home', component: LandingPageComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'kyc', component: KnowCustomerComponent},
  {path: 'membersportal', component: HomeComponent, canActivate: [AuthGuard],
  children: [
    {path: '', component: WelcomeComponent},
    {path: 'welcome', component: WelcomeComponent},
    {path: 'profile', component: ProfileComponent}
  ]},
  {path: '', redirectTo: '', pathMatch: 'full'},
  {path: '**', component: LandingPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
