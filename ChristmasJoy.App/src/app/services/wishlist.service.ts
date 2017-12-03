import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { Observable } from 'rxjs/Rx';
import { WishListItem } from '../models/WishListItem';

@Injectable()
export class WishlistService extends BaseService{
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

    saveItem(item: WishListItem): Observable<any>{
        if(item.id == '0'){
          return this.http.post(this.baseUrl + "/wishList/add/", item, this.requestOptions)
          .map(response => response.json())
          .catch(this.handleError);
        } else{
          return this.http.post(this.baseUrl + "/wishList/update/", item, this.requestOptions)
          .catch(this.handleError);
        }
    }

    deleteItem(item: WishListItem): Observable<any>{
        return this.http.post(this.baseUrl + "/wishList/delete", item, this.requestOptions)
        .catch(this.handleError);
    }

}