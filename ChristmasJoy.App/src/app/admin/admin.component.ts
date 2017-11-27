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
}
