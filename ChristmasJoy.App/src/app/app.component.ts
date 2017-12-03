import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  isLoggedIn: boolean = false;
  userId: number;
  subscription: Subscription;
  userSubscription: Subscription;

  isAdmin: boolean = false;

  constructor(private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => this.isLoggedIn = status);
    this.userSubscription = this.authService.authCurrentUser$
                                            .subscribe(user=> {
                                              this.isAdmin = user.IsAdmin;
                                              this.userId = user.Id;
                                            });
    
  }

  ngOnDestroy(){
    this.subscription.unsubscribe();
    this.userSubscription.unsubscribe();
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  goToProfile(){
    this.router.navigate(['/profile', this.userId]);
  }
}
