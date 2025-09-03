import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { BoardEntity } from "@app/features/board/models/board.model";
import { Observable } from "rxjs";
import { ColumnEntity } from "../models/column.model";
import { Log } from "../models/log.model";

@Injectable({
    providedIn: 'root'
})
export class SelectedBoardService {

    constructor(
        private apiService: ApiService,
    ) { }

    getBoardById(id: string): Observable<BoardEntity> {
        return this.apiService.get<BoardEntity>(`board/${id}`)
    }

    getColumnsByBoardId(boardId: string): Observable<ColumnEntity[]> {
        return this.apiService.get<ColumnEntity[]>(`column/${boardId}`)
    }

    getLogsByBoardId(boardId: string): Observable<Log[]> {
        return this.apiService.get<Log[]>(`log/${boardId}/0`)
    }

    getPagedLogs(boardId: string, page: number): Observable<Log[]> {
        return this.apiService.get<Log[]>(`log/${boardId}/${page}`)
    }
}