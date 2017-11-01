import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { LoggedInUser } from '../models/LoggedInUser';

@Injectable()
export class AuthService extends BaseService {
  private loggedIn = false;
  private baseUrl: string = '';
  private currentUser;

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    this.currentUser = new LoggedInUser();

    if (this.loggedIn) {
      this.currentUser.Email = localStorage.getItem('email');
      this.currentUser.IsAdmin = localStorage.getItem('isAdmin');
      this.currentUser.UserName = localStorage.getItem('userName');
    }
    this.baseUrl = configService.getApiURI();
  }

  login(email, password) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return this.http
      .post(this.baseUrl + '/account', JSON.stringify({ 'Email': email, 'Password': password }))
      .map(res => res.json())
      .map(res => {
          localStorage.setItem('auth_token', res.auth_token);
          this.loggedIn = true;
          localStorage.setItem('email', email);          
          localStorage.setItem('isAdmin', res.admin);
          localStorage.setItem('userName', res.userName);

          this.currentUser.Email = email;
          this.currentUser.IsAdmin = res.admin;
          this.currentUser.UserName = res.userName;

        return true;
      })
      .catch(this.handleError);
  }

  getCurrentUser() {
    return this.currentUser;
  }

  isLoggedIn() {
    return this.loggedIn;
  }  
}
