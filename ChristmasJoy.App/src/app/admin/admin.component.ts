import { Component, OnInit } from '@angular/core';
import { User } from '../models/User';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'admin-dashboard',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  users: User[];
  errorMessage: string;
  alertMessage: string;
  editMode: boolean = false;
  editUserId: number = null;
  oldUser: User;

  constructor(private usersService: UsersService) 
  { 
    this.errorMessage = "";
  }

  ngOnInit(){
    this.usersService
        .getAllUsers()
        .subscribe((data: User[]) => 
        {
          this.users = data;
        },
      error => {
        this.errorMessage= error;
      });
  }

  addNewUser() {
    if(this.editMode){
      return;
    }

    let editUser = new User();
    editUser.age = Math.floor(Math.random()*(200-100+1)+100);
    editUser.customId = 0;
    editUser.isAdmin = false;
    editUser.secretSantaForId = null;
    this.editUserId = 0;
    this.users.unshift(editUser);
  }

  cancelEdit() {   
    if(this.editUserId == 0){
      this.users.shift();
      this.editUserId = null;
      return;
    }

    for(let user of this.users){
      if(user.customId == this.editUserId){
        user.email = this.oldUser.email;
        user.userName = this.oldUser.userName;
        this.editUserId = null;
        return;
      }
    }
  }

  editUser(index){
    if(this.users[index]){
      this.oldUser = new User();
      this.oldUser.email =  this.users[index].email;
      this.oldUser.userName = this.users[index].userName;
      this.editUserId = this.users[index].customId;
    }
  }

  saveUser(user){
    this.errorMessage = "";
    this.alertMessage = "";

    this.usersService.saveUser(user).subscribe(
      response => {
        if(response && response.id && response.customId){
          user.id = response.id;
          user.customId = response.customId;
        }
        this.editUserId = null;
        this.alertMessage = "User changes were successfully saved.";
      },
      err => {
        this.errorMessage = err;
      }
    )
  }
}
