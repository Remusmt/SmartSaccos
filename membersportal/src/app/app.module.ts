import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { SpinnerComponent } from './shared/spinner/spinner.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './shared/material/material.module';
import { JwtInterceptor } from './shared/services/jwt.interceptor';
import { LandingPageComponent } from './site/landing-page/landing-page.component';
import { HeaderComponent } from './site/layout/header/header.component';
import { FooterComponent } from './site/layout/footer/footer.component';
import { AboutusComponent } from './site/pages/aboutus/aboutus.component';
import { ProductsComponent } from './site/pages/products/products.component';
import { CarouselComponent } from './site/pages/carousel/carousel.component';
import { ContactsComponent } from './site/pages/contacts/contacts.component';
import { CanjoinComponent } from './site/pages/canjoin/canjoin.component';
import { RegisterComponent } from './views/register/register.component';
import { LoginComponent } from './views/login/login.component';
import { HomeComponent } from './views/home/home.component';
import { KnowCustomerComponent } from './views/know-customer/know-customer.component';

@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent,
    SpinnerComponent,
    LandingPageComponent,
    HeaderComponent,
    FooterComponent,
    AboutusComponent,
    ProductsComponent,
    CarouselComponent,
    ContactsComponent,
    CanjoinComponent,
    RegisterComponent,
    LoginComponent,
    HomeComponent,
    KnowCustomerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
