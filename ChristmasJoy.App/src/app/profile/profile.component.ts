import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UsersService } from '../services/users.service';
import { LoggedInUser } from '../models/LoggedInUser';
import { UserData } from '../models/UserData';
import { WishListItem } from '../models/WishListItem';
import { Comment } from '../models/Comment';
import { WishlistService } from '../services/wishlist.service';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private userSubscription: Subscription;
  private currentUserId: number;
  private userId: number;
  private userData: UserData;
  public errorMessage: string = '';
  public userStatus: string = 'elf'; 
  private isEditingWishlist: boolean =  false;
  private isSantaPanelOpen: boolean = false;
  private loadingSanta: boolean = false;

  constructor(private authSrv: AuthService,
    private usersSrv: UsersService,
    private wishlistSrv: WishlistService,
    private route: ActivatedRoute,
    private router: Router) {
      let id;
      this.userData = null;
      this.route.params.subscribe(params=>{
        id = +params.id;

        if(!id || id == 0){
          this.errorMessage = "User profile not found."
       }
       else{
         this.userId = id;
         this.getUserData();
       }
      })
    }

  isSelfProfile(){
    return this.currentUserId == this.userId && this.userData;
  }

  getUserData(){
    this.usersSrv.getAllUserData(this.userId)
    .subscribe(
      response => {
        if(response){
          this.userData = response;
        }
      },
      error => {this.errorMessage = error;}
    )
  }

  ngOnInit(){
    this.userSubscription = this.authSrv.authCurrentUser$
    .subscribe(user=> {
      this.currentUserId = user.Id;
    });   
  }

  canEdit(){
    return this.isSelfProfile() && !this.isEditingWishlist;
  }

  canAdd(){
    return this.canEdit() 
            && this.userData.wishList.length < 10;
  }
  addToWishlist(){
    let newItem = new WishListItem();
    newItem.id = "0";
    newItem.isEdited =  true;
    newItem.item = "";
    newItem.userId = this.userId;
    this.isEditingWishlist = true;
    this.userData.wishList.push(newItem);
   }

  closewishlistedit(item, i){
    item.isEdited = false;
    if(item.id == 0){
      //was new so we have to remove it.
      this.userData.wishList.splice(i, 1);
    }
    this.isEditingWishlist = false;
  }

  editwishlist(item, i){
    item.isEdited = true;
    this.isEditingWishlist = true;
  }

  deletewishlist(item, i){
    this.wishlistSrv.deleteItem(item)
    .subscribe(
      response => {
        this.userData.wishList.splice(i, 1);
      },
      error => { this.errorMessage = error; }
    );
  }

  savewishlist(item, i){
    this.wishlistSrv.saveItem(item)
    .subscribe(
      response => {
        if(response && response.id){
          item.id = response.id;
        }
        item.isEdited = false;
        this.isEditingWishlist = false;
      },
      error => {this.errorMessage = error;}
    )
  }

  generateSecretReceiver(){
    this.loadingSanta = true;
    this.usersSrv.setSecretSanta(this.userId)
    .subscribe(
      response => {
        if(response){
          this.userData.userInfo.secretSantaForId = response.receiverId;
          this.userData.userInfo.secretSantaFor = response.receiverName;
          this.isSantaPanelOpen = true;
          this.loadingSanta = false;
        }
      },
      error => {
        this.errorMessage = error;
        this.loadingSanta = false;
      }
    )
  }

  toggleSantaPanelOpen(event: MouseEvent): void {
    event.preventDefault();
    this.isSantaPanelOpen = !this.isSantaPanelOpen;
  }
}
