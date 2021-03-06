import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuardService as AuthGuard } from './services/auth.guard';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthService } from './services/auth.service';
import { ConfigService } from './services/config.service';
import { ImportComponent } from './admin/import/import.component';

const appRoutes: Routes = [
    { path: '', 
      component: DashboardComponent,
      canActivate: [AuthGuard] 
    },
    { path: 'login', component: LoginComponent },
    {
      path: 'profile/:id',
      component: ProfileComponent,
      canActivate: [AuthGuard]
    },
    {
      path: 'admin',
      component: AdminComponent,
      canActivate: [AuthGuard],
      data: {
        expectedRole: 'Admin'
      },
    },
    {
      path: 'import',
      component: ImportComponent,
      canActivate: [AuthGuard],
      data: {
        expectedRole: 'Admin'
      },
    },
    { path: '**', redirectTo: '' }
  ];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);