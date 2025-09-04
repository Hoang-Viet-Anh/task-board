import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { LoginForm } from "../models/login.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class LoginService {
    constructor(private readonly apiService: ApiService) { }

    performLogin(loginForm: LoginForm): Observable<any> {
        return this.apiService.post<any>('auth/login', loginForm);
    }
}