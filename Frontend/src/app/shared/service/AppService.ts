import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiUrlBuilder } from './ApiUrlBuilder';
import { Observable } from 'rxjs';
import { User } from '../models/User';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AppService {
    private JWT: string;

    constructor(
        private http: HttpClient,
        private urlBuilder: ApiUrlBuilder
    ){} 

    getRecommendations(): Observable<any> {
        return this.http.get(this.urlBuilder.recommendationsUrl());
    }

    login(username:string, password:string):Observable<User>{
        const headerDict = {
            'username': username,
            'password': password
        }

        const requestOptions = {
            headers: new HttpHeaders(headerDict)
        };

        return this.http.get<any>(this.urlBuilder.tokenUrl(), requestOptions)
            .pipe(map(token => {

            if (token) {  
                const helper = new JwtHelperService();
                const decodedToken = helper.decodeToken(token);
                var user = new User();
                user.Username = username;
                user.Email = decodedToken.email;
                user.Token = token;

                localStorage.setItem('currentUser', JSON.stringify(user));
                return user;
            }
        
            return null;
        }));
    }

    logout() {
        localStorage.removeItem('currentUser');
    }

    register(username:string, email:string, password:string):Observable<User>{
        const headerDict = {
            'username': username,
            'email': email,
            'password': password
        }

        const requestOptions = {
            headers: new HttpHeaders(headerDict)
        };

        return this.http.get<any>(this.urlBuilder.registerUrl(), requestOptions)
            .pipe(map(token => {

            if (token) {  
                const helper = new JwtHelperService();
                const decodedToken = helper.decodeToken(token);
                
                var user = new User();
                user.Username = username;
                user.Email = decodedToken.email;
                user.Token = token;

                localStorage.setItem('currentUser', JSON.stringify(user));
                return user;
            }
        
            return null;
        }));
    }

    getGenres():Observable<any>{
        return this.http.get(this.urlBuilder.genres());
    }

    getArtists(value:string):Observable<any>{
        const headerDict = {
            'artist': value
        }

        const requestOptions = {
            headers: new HttpHeaders(headerDict)
        };

        return this.http.get(this.urlBuilder.artists(),requestOptions);
    }

    insertUserArtists(username:string,artists:any[]){
        const data = {
            'username': username,
            'artists': artists
        }
        return this.http.post(this.urlBuilder.insertUserArtists(),data).subscribe();
    }

    insertUserGenres(username:string,genres:string[]){
        const data = {
            'username': username,
            'genres': genres
        }
        return this.http.post(this.urlBuilder.insertUserGenres(),data).subscribe();
    }

    updateUser(username:string,email:string){
        const data = {
            'username': username,
            'email': email
        }
        return this.http.post(this.urlBuilder.updateUser(),data).subscribe();//() =>{
        //     let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        //     if(currentUser)
        //     {
        //       var user = new User();
        //       //user.Username = this.usernameValue;
        //       //user.Email = this.emailValue
        //       user.Token = currentUser.Token;
      
        //       localStorage.setItem('currentUser', JSON.stringify(user));
        //     }
        //     else
        //     {
        //       //this.openSnackBar("An error has occured!");
        //       this.logout();
        //       //this.router.navigate([]);
        //     }
        //   });
    }
}