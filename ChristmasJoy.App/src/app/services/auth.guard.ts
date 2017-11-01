import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot) {
    const expectedRole = route.data.expectedRole;

    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }
    var currentUser = this.authService.getCurrentUser();

    if (expectedRole == 'Admin' && !currentUser.IsAdmin) {
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
