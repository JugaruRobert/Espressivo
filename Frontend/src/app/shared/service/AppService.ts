import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiUrlBuilder } from './ApiUrlBuilder';
import { Observable, Subscription, interval } from 'rxjs';
import { User } from '../models/User';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable()
export class ProgressTimeout{
    private progressTimeout:Subscription = null;

    getTimeout():Subscription{
        return this.progressTimeout;
    }

    setTimeout(timeout:Subscription){
        this.progressTimeout = timeout;
    }

    subscribe(callback:any){
        if(!this.progressTimeout)
            this.progressTimeout = interval(1000).subscribe(()=>{callback();});
    }

    unsubscribe(){
        if(this.progressTimeout)
            this.progressTimeout.unsubscribe();
        this.progressTimeout = null;
    }
}

@Injectable()
export class AppService {
    private JWT: string;
   
    constructor(
        private http: HttpClient,
        private urlBuilder: ApiUrlBuilder
    ){} 

    getRecommendations(image:any): Observable<any> {
        const formData: FormData = new FormData();
        formData.append('Image', image, "imageName");
        return this.http.post(this.urlBuilder.recommendationsUrl(), formData);
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
                user.ID = decodedToken.nameid;
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
                user.ID = decodedToken.nameid;
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

    insertUserArtists(userID:string,artists:any[]){
        const data = {
            'userID': userID,
            'artists': artists
        }
        return this.http.post(this.urlBuilder.insertUserArtists(),data);
    }

    insertUserGenres(userID:string,genres:string[]){
        const data = {
            'userID': userID,
            'genres': genres
        }
        return this.http.post(this.urlBuilder.insertUserGenres(),data);
    }

    updateUser(userID:string,username:string,email:string){
        const data = {
            'userID':userID,
            'username': username,
            'email': email
        }
        return this.http.post(this.urlBuilder.updateUser(),data);
    }

    getPreferences(userID:string):Observable<any>{
        const headerDict = {
            'userID': userID
        }

        const requestOptions = {
            headers: new HttpHeaders(headerDict)
        };

        return this.http.get(this.urlBuilder.preferences(),requestOptions);
    }
}