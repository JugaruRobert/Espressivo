import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiUrlBuilder } from './ApiUrlBuilder';
import { Observable } from 'rxjs';


@Injectable()
export class AppService {
    constructor(
        private http: HttpClient,
        private urlBuilder: ApiUrlBuilder
    ){} 

    getRecommendations(): Observable<any> {
        return this.http.get(this.urlBuilder.recommendationsUrl());
    }
}