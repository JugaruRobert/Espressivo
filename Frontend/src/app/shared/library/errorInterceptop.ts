import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AppService } from '../service/AppService';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private service: AppService,private snackBar: MatSnackBar,private router:Router) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            this.service.logout();
            if (err.status === 406) {
                switch(err.error.Message){
                    case "Error.ExistingUsername":
                    {
                        //$("#registerForm").find("input[formcontrolname='username']").val("")
                        this.openSnackBar("There is already an user with this username. Please choose a different one and try again!");
                        break;
                    }
                    case "Error.ExistingEmail":
                    {
                        //$("#registerForm").find("input[formcontrolname='email']").val("")
                        this.openSnackBar("There is already an user with this email. Please choose a different one and try again!");
                        break;
                    }
                }
            }
            
            if (err.status === 401) {
                switch(err.error.Message){
                    case "Error.MissingCredentials":
                    {
                        this.openSnackBar("Some of the required credentials were missing!");
                        break;
                    }
                    case "Error.InvalidCredentials":
                    {
                        //$("#loginForm").find("input[formcontrolname='username']").val("")
                        //$("#loginForm").find("input[formcontrolname='password']").val("")
                        this.openSnackBar("Invalid username or password!");
                        break;
                    }
                    default:  
                    {
                        this.router.navigate([]);
                        this.openSnackBar("You are not authorized to perform this action!");
                    }
                }
            }

            const error = err.error.message || err.statusText;
            return throwError(error);
        }))
    }

    private openSnackBar(message: string) {
        this.snackBar.open(message, '', {verticalPosition: "top", duration: 6000});
      }
}