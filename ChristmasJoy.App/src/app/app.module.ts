import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, XHRBackend } from '@angular/http';
import { AuthenticateXHRBackend } from './services/authenticate-xhr.backend';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService as AuthGuard } from './services/auth.guard';

import { routing } from './app.routing';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './dashboard//dashboard.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthService } from './services/auth.service';
import { ConfigService } from './services/config.service';
import { UsersService } from './services/users.service';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    LoginComponent,
    DashboardComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    routing
  ],
  providers: [ 
    AuthGuard, 
    ConfigService, 
    AuthService,
    UsersService,
    { 
      provide: XHRBackend, 
      useClass: AuthenticateXHRBackend
    }
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
