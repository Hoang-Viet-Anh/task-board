import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class HeaderService {
    constructor(private readonly apiService: ApiService) { }

    logout(): Observable<any> {
        return this.apiService.post('auth/logout', {})
    }
}