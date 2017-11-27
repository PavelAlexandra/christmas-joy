import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private model :any = {};
  private loading: boolean = false;
  private returnUrl: string;
  private errorMessage: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService) { 
       console.log('this is login component');
       this.errorMessage = "";
     }

  ngOnInit(){
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login(){
    this.loading = true;
    this.errorMessage = "";
    this.authService.login(this.model.email, this.model.password)
      .subscribe(data => {
          this.router.navigate([this.returnUrl]);
      },
      error => {
        this.errorMessage = error;
        this.loading = false;
      })
  }
}
