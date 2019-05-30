import { Injectable } from '@angular/core';

@Injectable()
export class ApiUrlBuilder {
    private readonly baseUrl = 'http://localhost:29852';
    
    recommendationsUrl():string{
        return this.baseUrl + "/recommendations";
    }

    tokenUrl():string{
        return this.baseUrl + "/token";
    }

    registerUrl():string{
        return this.baseUrl + "/users/register";
    }

    genres():string{
        return this.baseUrl + "/recommendations/genres";
    }

    artists():string{
        return this.baseUrl + "/recommendations/artists/";
    }

    insertUserGenres(): string {
        return this.baseUrl + "/userGenres";
    }

    insertUserArtists(): string {
        return this.baseUrl + "/userArtists";
    }

    updateUser():string{
        return this.baseUrl + "/users/update";
    }
}