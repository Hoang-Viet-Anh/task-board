import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environments';

@Injectable({
    providedIn: 'root',
})
export class ApiService {
    private readonly apiUrl = environment.apiUrl;

    constructor(public readonly http: HttpClient) { }

    get<T>(endpoint: string, options?: object): Observable<T> {
        return this.http.get<T>(`${this.apiUrl}${endpoint}`, {
            withCredentials: true,
            ...options
        },);
    }

    post<T>(endpoint: string, body: any, options?: object): Observable<T> {
        return this.http.post<T>(`${this.apiUrl}${endpoint}`, body, {
            withCredentials: true,
            ...options
        });
    }

    put<T>(endpoint: string, body: any, options?: object): Observable<T> {
        return this.http.put<T>(`${this.apiUrl}${endpoint}`, body, {
            withCredentials: true,
            ...options
        });
    }

    delete<T>(endpoint: string, options?: object): Observable<T> {
        return this.http.delete<T>(`${this.apiUrl}${endpoint}`, {
            withCredentials: true,
            ...options
        });
    }

    patch<T>(endpoint: string, body: any, options?: object): Observable<T> {
        return this.http.patch<T>(`${this.apiUrl}${endpoint}`, body, {
            withCredentials: true,
            ...options
        });
    }
}
