import { BehaviorSubject, Observable } from 'rxjs/Rx';
import { Headers, Http, RequestOptions, Response } from '@angular/http';

import { BaseService } from './base.service';
import { ConfigService } from './config.service';
import { Injectable } from '@angular/core';
import { LoggedInUser } from '../models/LoggedInUser';
import { User } from '../models/User';

@Injectable()
export class UsersService extends BaseService {
  private baseUrl: string;
  private requestOptions: RequestOptions;
  private uploadRequestOptions: RequestOptions;

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.baseUrl = configService.getApiURI();
    let headers = configService.getHeaders();
    let authToken = localStorage.getItem('auth_token');
    let uploadHeaders = new Headers();
   // uploadHeaders.append('Content-Type',' "multipart/form-data; boundary');
    uploadHeaders.append('Access-Control-Allow-Origin', '*');

    if(authToken){
        headers.append('Authorization', `Bearer ${authToken}`);
        uploadHeaders.append('Authorization', `Bearer ${authToken}`);
    }
    this.requestOptions = new RequestOptions({headers: headers});
    this.uploadRequestOptions = new RequestOptions({headers: uploadHeaders});
  }

  getAllUsers(): Observable<User[]>{
    return this.http.get(this.baseUrl + "/logins", this.requestOptions)
    .map(response => response.json())
    .catch(this.handleError);
  }

  saveUser(user: User): Observable<any>{
      if(user.id == 0){
        return this.http.post(this.baseUrl + "/logins/AddUser/", user, this.requestOptions)
        .map(response => response.json())
        .catch(this.handleError);
      } else{
        return this.http.post(this.baseUrl + "/logins/UpdateUser/", user, this.requestOptions)
        .catch(this.handleError);
      }
  }

  getAllUserData(id: number): Observable<any>{
    return this.http.get(this.baseUrl + "/users/getUserData/" + id, this.requestOptions)
    .map(response => response.json())
    .catch(this.handleError);
  }

  setSecretSanta(santaUserId: number): Observable<any>{
    return this.http.get(this.baseUrl + "/users/getReceiver/" + santaUserId, this.requestOptions)
    .map(response => response.json())
    .catch(this.handleError);
  }

  getUsersStatusMap(): Observable<any>{
    return this.http.get(this.baseUrl + "/users/getMap/", this.requestOptions)
    .map(response => response.json())
    .catch(this.handleError);
  }

  getAllUserStatuses(): Observable<any>{
    return this.http.get(this.baseUrl + "/users/getStatuses/", this.requestOptions)
    .map(response => response.json())
    .catch(this.handleError);
  }

  uploadFile(file: File, password: string): Observable<any>{
    const formData: FormData = new FormData();
    formData.append('password', password);
    formData.append('file', file, file.name);   
  
    return this.http.post(this.baseUrl + "/logins/UploadUsers/",formData, this.uploadRequestOptions)
    .map(response => response.json())
    .catch(this.handleError);

  }
}
