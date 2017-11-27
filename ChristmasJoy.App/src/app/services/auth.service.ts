import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { LoggedInUser } from '../models/LoggedInUser';

import { BehaviorSubject } from 'rxjs/Rx';

@Injectable()
export class AuthService extends BaseService {
  private loggedIn = false;
  private baseUrl: string = '';
  private currentUser: LoggedInUser;

   // Observable navItem source
   private _authNavStatusSource = new BehaviorSubject<boolean>(false);
   private _authCurrentUserSource= new BehaviorSubject<LoggedInUser>(new LoggedInUser());
   // Observable navItem stream
   authNavStatus$ = this._authNavStatusSource.asObservable();
   authCurrentUser$ = this._authCurrentUserSource.asObservable();
 
  constructor(private http: Http, private configService: ConfigService) {
    super();
    let token = localStorage.getItem('auth_token');
    if(token){
      this.loggedIn = true;
    }
    this.currentUser = new LoggedInUser();

    if (this.loggedIn) {
      this.currentUser.Email = localStorage.getItem('email');
      this.currentUser.IsAdmin = localStorage.getItem('isAdmin') ? true: false;
      this.currentUser.UserName = localStorage.getItem('userName');
    }
    this._authNavStatusSource.next(this.loggedIn);
    this._authCurrentUserSource.next(this.currentUser);
    this.baseUrl = configService.getApiURI();
  }

  login(email, password) {
    let options = new RequestOptions({ headers: this.configService.getHeaders() });

    return this.http
      .post(this.baseUrl + '/authorize', 
            JSON.stringify({ 'Email': email, 'Password': password }),
            options)
      .map(res => res.json())
      .map(res => {
          localStorage.setItem('auth_token', res.access_token);
          this.loggedIn = true;
          this._authNavStatusSource.next(this.loggedIn);
          localStorage.setItem('email', email);          
          localStorage.setItem('isAdmin', res.admin);
          localStorage.setItem('userName', res.userName);

          this.currentUser.Email = email;
          this.currentUser.IsAdmin = res.admin;
          this.currentUser.UserName = res.userName;
          
          this._authCurrentUserSource.next(this.currentUser);

          return true;
      })
      .catch(this.handleError);
  }

  logout() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('email');
    localStorage.removeItem('isAdmin');
    localStorage.removeItem('userName');

    this.loggedIn = false;
    this._authNavStatusSource.next(false);
  }

  getCurrentUser() {
    return this.currentUser;
  }

  isLoggedIn() {
    return this.loggedIn;
  }  
}
