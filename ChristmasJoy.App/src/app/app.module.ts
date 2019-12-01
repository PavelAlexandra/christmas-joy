import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, XHRBackend } from '@angular/http';
import { AuthenticateXHRBackend } from './services/authenticate-xhr.backend';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService as AuthGuard } from './services/auth.guard';

import { routing } from './app.routing';
import { MomentModule } from 'angular2-moment';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './dashboard//dashboard.component';
import { ImportComponent } from './admin/import/import.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthService } from './services/auth.service';
import { ConfigService } from './services/config.service';
import { UsersService } from './services/users.service';
import { WishlistService } from './services/wishlist.service';
import { CommentsService } from './services/comments.service';
import { FeedbackComponent } from './components/feedback.component';
import { CountdownComponent } from './components/countdown.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    LoginComponent,
    DashboardComponent,
    ImportComponent,
    ProfileComponent,
    FeedbackComponent,
    CountdownComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    MomentModule
  ],
  providers: [ 
    AuthGuard, 
    ConfigService, 
    AuthService,
    UsersService,
    WishlistService,
    CommentsService,
    { 
      provide: XHRBackend, 
      useClass: AuthenticateXHRBackend
    }
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
