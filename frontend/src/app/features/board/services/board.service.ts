import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { Observable } from "rxjs";
import { BoardEntity } from "../models/board.model";

@Injectable({
    providedIn: 'root'
})
export class BoardService {
    constructor(private readonly apiService: ApiService) { }

    getBoards(): Observable<BoardEntity[]> {
        return this.apiService.get<BoardEntity[]>('board')
    }

    removeBoard(id: string): Observable<any> {
        return this.apiService.delete<any>(`board/leave/${id}`)
    }

    updateBoard(board: BoardEntity): Observable<any> {
        return this.apiService.post<any>('board/update', board)
    }
}