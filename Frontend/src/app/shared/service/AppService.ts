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

        return this.http.get(this.urlBuilder.artists());
    }

    insertUserArtists(username:string,artists:any[]):Observable<any>{
        const data = {
            'username': username,
            'artists':artists
        }
        return this.http.post(this.urlBuilder.insertUserArtists(),data);
    }

    insertUserGenres(username:string,genres:any[]):Observable<any>{
        const data = {
            'username': username,
            'genreNames':genres
        }
        return this.http.post(this.urlBuilder.insertUserGenres(),data);
    }
}