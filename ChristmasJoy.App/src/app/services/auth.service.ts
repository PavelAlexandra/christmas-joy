import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { LoggedInUser } from '../models/LoggedInUser';

import { BehaviorSubject } from 'rxjs/Rx';
import * as jwt_decode from 'jwt-decode';

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
    
    let tokenExpired = this.isTokenExpired();

    if (!tokenExpired) {
     this.setCurrentUser(null);
    }
    this._authNavStatusSource.next(!tokenExpired);
    this._authCurrentUserSource.next(this.currentUser);
    this.baseUrl = configService.getApiURI();
  }

  setCurrentUser(token: string){
    if(token === undefined || token == null) token = this.getToken();
    if(token === "undefined" || token == null) this.currentUser = null;
    
      const decoded = jwt_decode(token);
      if(decoded){
        this.currentUser = new LoggedInUser();
        this.currentUser.IsAdmin = decoded.role === "Admin";
        this.currentUser.Email = decoded.sub;
        this.currentUser.Id = +decoded.id;
      }else{
        this.currentUser = null;
      }
  }

  getTokenExpirationDate(token: string): Date {
    if(token === undefined || token == null) token = this.getToken();
    if(token === "undefined" || token == null) return null;
     const decoded = jwt_decode(token);

      if (decoded.exp === undefined) return null;

      const date = new Date(0); 
      date.setUTCSeconds(decoded.exp);
      return date;
  }

  getToken(): string {
    return localStorage.getItem('auth_token');
  }

  setToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  isTokenExpired(token?: string): boolean {
    if(token === undefined || token == null) token = this.getToken();
    if(token === "undefined" || token == null) return true;

    const date = this.getTokenExpirationDate(token);
    if(date === undefined || date == null) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  login(email, password) {
    let options = new RequestOptions({ headers: this.configService.getHeaders() });

    return this.http
      .post(this.baseUrl + '/authorize', 
            JSON.stringify({ 'Email': email, 'Password': password }),
            options)
      .map(res => res.json())
      .map(res => {
          this.setToken(res.access_token);
          this.setCurrentUser(res.access_token);

          this._authNavStatusSource.next(true);
          this._authCurrentUserSource.next(this.currentUser);

          return true;
      })
      .catch(this.handleError);
  }

  logout() {
    localStorage.removeItem('auth_token');
    this._authNavStatusSource.next(false);
  }

  getCurrentUser() {
    return this.currentUser;
  }

  isLoggedIn() {
    return !this.isTokenExpired();
  }  
}
