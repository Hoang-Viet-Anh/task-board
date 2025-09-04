import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";

@Injectable({ providedIn: 'root' })
export class AuthService {
    constructor(private readonly apiService: ApiService) { }

    refreshTokens() {
        return this.apiService.post('auth/refresh-tokens', {});
    }
}