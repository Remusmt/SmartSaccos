import { HomeComponent } from './views/home/home.component';
import { LoginComponent } from './views/login/login.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { SpinnerComponent } from './shared/spinner/spinner.component';
import { AvatorComponent } from './views/avator/avator.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './shared/services/jwt.interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './shared/material/material.module';
import { MembersMainComponent } from './views/members/members-main/members-main.component';
import { MembersListComponent } from './views/members/members-list/members-list.component';
import { MembersDetailsComponent } from './views/members/members-details/members-details.component';
import { MembersMainListComponent } from './views/members/members-main-list/members-main-list.component';
import { RegistrationActionComponent } from './views/registration-action/registration-action.component';
import { MemberProfileComponent } from './views/members/member-profile/member-profile.component';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { FileUploadComponent } from './views/members/file-upload/file-upload.component';

@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent,
    SpinnerComponent,
    LoginComponent,
    HomeComponent,
    AvatorComponent,
    MembersMainComponent,
    MembersListComponent,
    MembersDetailsComponent,
    MembersMainListComponent,
    RegistrationActionComponent,
    MemberProfileComponent,
    FileUploadComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxDropzoneModule,
    MaterialModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
