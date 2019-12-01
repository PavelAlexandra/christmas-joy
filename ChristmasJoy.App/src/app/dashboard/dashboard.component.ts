import { Component, OnDestroy, OnInit } from '@angular/core';

import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { UserData } from '../models/UserData';
import { UserStatus } from '../models/UserStatus';
import { UsersService } from '../services/users.service';

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
  public secretSantaDate: string = "December 15, 2019 19:00:00";
  public christmasDate: string = "December 25, 2019 00:00:00";

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

  isPassedSecretSanta(){
    let date = new Date(this.secretSantaDate);
    return (date.valueOf() < new Date().valueOf())
  }

  isPassedChristmas(){
    let date = new Date(this.christmasDate);
    return (date.valueOf() < new Date().valueOf())
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
