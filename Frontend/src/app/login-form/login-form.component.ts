import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { AppService } from '../shared/service/AppService';
import { ApiUrlBuilder } from '../shared/service/ApiUrlBuilder';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss'],
  providers:[AppService,ApiUrlBuilder]
})

export class LoginFormComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  private usernameValue:string;
  private passwordValue:string;

  @ViewChild('loginFormDirective') loginFormDirective;
  
  constructor(private formBuilder: FormBuilder,private service:AppService,private router: Router,private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
      this.submitted = true;

      if (this.loginForm.invalid) {
          return;
      }

      this.usernameValue = this.loginForm.get('username').value;
      this.passwordValue = this.loginForm.get('password').value;

      this.service.login(this.usernameValue, this.passwordValue).subscribe( user => {
        if(user != null) {
            this.router.navigate(['dashboard']);
        }
        else
        {
            this.openSnackBar("An error has occured. Please try again!");
            this.clearForm();
        }
    });
  }

  clearErrors(){
      this.loginFormDirective.resetForm();
      this.loginForm.markAsPristine();
      this.loginForm.markAsUntouched();
      this.loginForm.updateValueAndValidity();
      this.loginForm.reset();
  }

  clearForm(){
    this.loginForm.get('username').setValue("");
    this.loginForm.get('password').setValue("");
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, '', {verticalPosition: "top", duration: 6000});
  }
}
