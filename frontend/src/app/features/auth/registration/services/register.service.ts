import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { Observable } from "rxjs";
import { RegisterForm } from "../models/register.model";

@Injectable({
    providedIn: 'root'
})
export class RegisterService {
    constructor(private readonly apiService: ApiService) { }

    performRegister(registerForm: RegisterForm): Observable<any> {
        return this.apiService.post<any>('auth/register', registerForm);
    }
}