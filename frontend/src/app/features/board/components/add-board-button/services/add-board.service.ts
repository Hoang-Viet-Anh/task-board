import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AddBoardService {
    constructor(private readonly apiService: ApiService) { }

    createBoard(title: string): Observable<any> {
        return this.apiService.post<any>('board/create', { boardTitle: title })
    }

    joinBoard(inviteCode: string): Observable<any> {
        return this.apiService.post<any>(`board/join/${inviteCode}`, {})
    }
}