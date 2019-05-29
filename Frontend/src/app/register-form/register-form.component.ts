import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { AppService } from '../shared/service/AppService';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  private usernameValue:string;
  private emailValue:string;
  private passwordValue:string;

  @ViewChild('registerFormDirective') registerFormDirective;

  constructor(private formBuilder: FormBuilder,private service:AppService,private router:Router,private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get f() { return this.registerForm.controls; }

  onSubmit() {
      this.submitted = true;

      if (this.registerForm.invalid) {
          return;
      }

      this.usernameValue = this.registerForm.get('username').value;
      this.passwordValue = this.registerForm.get('password').value;
      this.emailValue = this.registerForm.get('email').value;

      this.service.register(this.usernameValue, this.emailValue, this.passwordValue).subscribe( user => {
        if(user != null) {
            this.router.navigate(['preferences']);
        }
        else
        {
          this.openSnackBar("An error has occured. Please try again!");
          this.clearForm();
        }
      });
  }

  clearErrors(){
    this.registerFormDirective.resetForm();
    this.registerForm.markAsPristine();
    this.registerForm.markAsUntouched();
    this.registerForm.updateValueAndValidity();
    this.registerForm.reset();
  }

  clearForm(){
    this.registerForm.get('username').setValue("");
    this.registerForm.get('email').setValue("");
    this.registerForm.get('password').setValue("");
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, '', {verticalPosition: "top", duration: 6000});
  }
}