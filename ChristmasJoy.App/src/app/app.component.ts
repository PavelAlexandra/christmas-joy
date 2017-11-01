import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private _httpService: Http) { }

  usersList: string[] = [];
  ngOnInit() {
    this._httpService.get('/api/users').subscribe(values => {
      this.usersList = values.json() as string[];
    })
  }
}
