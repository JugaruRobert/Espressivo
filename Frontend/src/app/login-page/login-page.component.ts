import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AppService } from '../shared/service/AppService';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginPageComponent implements OnInit {
  @ViewChild('loginForm') loginForm;
  @ViewChild('registerForm') registerForm;

  constructor(private formBuilder: FormBuilder,private service:AppService) { }

  ngOnInit() {
    this.service.logout();
  }

  onTabChange(){
    this.loginForm.clearErrors();
    this.registerForm.clearErrors();
  }
}
