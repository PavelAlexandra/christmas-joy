import { Component, 
         OnInit, 
         OnDestroy, 
         ChangeDetectorRef,
         ChangeDetectionStrategy } from '@angular/core';
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
  isMenuOpen: boolean = false;

  constructor(private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => this.isLoggedIn = status);
    this.userSubscription = this.authService.authCurrentUser$
                                            .subscribe(user=> {
                                              if(user){
                                                this.isAdmin = user.IsAdmin;
                                                this.userId = user.Id;
                                              }
                                            });
    
  }

  toggleMenu($event){
    if($event.handled === false) return
    $event.stopPropagation();
    $event.preventDefault();
    this.isMenuOpen = !this.isMenuOpen;
    $event.handled = true;
  }
  closeMenu(){
    this.isMenuOpen = false;
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
