import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { Observable } from 'rxjs/Rx';
import { Comment } from '../models/Comment';

@Injectable()
export class CommentsService extends BaseService{
    private baseUrl: string;
    private requestOptions: RequestOptions;
  
    constructor(private http: Http, private configService: ConfigService) {
      super();
      this.baseUrl = configService.getApiURI();
      let headers = configService.getHeaders();
      let authToken = localStorage.getItem('auth_token');
      if(headers && headers.get("Authorization")){
          headers.delete("Authorization");
      }

      if(authToken){
          headers.append('Authorization', `Bearer ${authToken}`);
      }
      this.requestOptions = new RequestOptions({headers: headers});
    }

    saveItem(item: Comment): Observable<any>{
        return this.http.post(this.baseUrl + "/comments/add/", item, this.requestOptions)
        .map(response => response.json())
        .catch(this.handleError);
    }

    getComments(userId: number, isPersonal: boolean): Observable<any>{
        return this.http.get(this.baseUrl + "/comments/all/" + userId, this.requestOptions)
        .map(response => response.json())
        .catch(this.handleError);
    }

    likeComment(userId: number, commentId: string){
        return this.http.post(this.baseUrl + "/comments/like/" + userId, {commentId: commentId}, this.requestOptions)
        .catch(this.handleError);
    }

}