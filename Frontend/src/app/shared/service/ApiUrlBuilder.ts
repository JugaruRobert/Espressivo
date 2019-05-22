import { Injectable } from '@angular/core';

@Injectable()
export class ApiUrlBuilder {
    private readonly baseUrl = 'http://localhost:29852/';
    
    recommendationsUrl():string{
        return this.baseUrl + "/recommendations";
    }
}