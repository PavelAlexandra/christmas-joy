import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'admin-dashboard',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {

  constructor() { }

  usersList: string[] = [];
}
