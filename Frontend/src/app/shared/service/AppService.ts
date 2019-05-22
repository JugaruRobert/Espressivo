import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiUrlBuilder } from './ApiUrlBuilder';


@Injectable()
export class AppService {
    constructor(
        private http: HttpClient,
        private urlBuilder: ApiUrlBuilder
    ){} 

    getRecommendations(): void {
        this.http.get(this.urlBuilder.recommendationsUrl()).subscribe(data => {
            console.log(data);
        },
        error => console.error(error));
    }
}