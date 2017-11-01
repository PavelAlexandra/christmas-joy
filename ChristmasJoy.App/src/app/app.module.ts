import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService as AuthGuard } from './services/auth.guard';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './dashboard//dashboard.component';
import { AuthService } from './services/auth.service';
import { ConfigService } from './services/config.service';

const appRoutes: Routes = [
  
  { path: '', component: AppComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'profile',
    component: AppComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRole: 'Admin'
    }
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  }
 
];

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    LoginComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [ AuthGuard, ConfigService, AuthService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
