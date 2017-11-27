import { Injectable } from '@angular/core';
import {Headers} from '@angular/http'; 

@Injectable()
export class ConfigService {  
    private _apiURI : string;
    private headers: Headers; 
 
    constructor() {
        this._apiURI = 'http://localhost:52929/api';
        this.headers= new Headers();
        this.headers.append('Content-Type','application/json');
        this.headers.append('Access-Control-Allow-Origin', '*');
     }
 
     getApiURI() {
         return this._apiURI;
     }    

     getHeaders(){
         return this.headers;
     }
}
