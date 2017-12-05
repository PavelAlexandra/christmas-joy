import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { UserData } from '../models/UserData';
import { UsersService } from '../services/users.service';
import { UserStatus } from '../models/UserStatus';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy{
  private userSubscription: Subscription;
  public currentUserId: number;
  public naughtyUsers: UserData[];
  public goodUsers: UserData[];
  public errorMessage: string;
  public isLoading: boolean = false;

  constructor(private authSrv: AuthService,
  private userSrv: UsersService) { }

  ngOnInit(){
    this.getUserStatuses();
   this.userSubscription = this.authSrv.authCurrentUser$
   .subscribe(user=> {
     if(user){
      this.currentUserId = user.Id;
     }
   }); 
  }

  ngOnDestroy(){
    this.userSubscription.unsubscribe();
  }

  getStatusImg(user: UserStatus){
    return '/assets/images/'+user.christmasStatus + '.jpg';
  }

  getUserStatuses(){
    this.isLoading = true;
    this.userSrv.getAllUserStatuses()
    .subscribe(
      response => {
        if(response && response.data){
          this.naughtyUsers = response.data.filter(x=> {return x.christmasStatus == 'grinch'});
          this.goodUsers = response.data.filter(x => {return x.christmasStatus != 'grinch'});
        }
        this.isLoading = false;
      },
      error => {
        this.errorMessage = error;
        this.isLoading = false;
      }
    )
  }

}
