import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { LoggedInUser } from '../models/LoggedInUser';

import { BehaviorSubject, Observable } from 'rxjs/Rx';
import { User } from '../models/User';

@Injectable()
export class UsersService extends BaseService {
  private baseUrl: string;
  private requestOptions: RequestOptions;

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.baseUrl = configService.getApiURI();
    let headers = configService.getHeaders();
    let authToken = localStorage.getItem('auth_token');
    
    if(authToken){
        headers.append('Authorization', `Bearer ${authToken}`);
    }
    this.requestOptions = new RequestOptions({headers: headers});
  }

  getAllUsers(): Observable<User[]>{
    return this.http.get(this.baseUrl + "/logins", this.requestOptions)
    .map(response => response.json())
    .catch(this.handleError);
  }
}
