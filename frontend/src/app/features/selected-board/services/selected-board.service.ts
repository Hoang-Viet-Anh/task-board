import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { BoardEntity } from "@app/features/board/models/board.model";
import { Observable } from "rxjs";
import { ColumnEntity } from "../models/column.model";

@Injectable({
    providedIn: 'root'
})
export class SelectedBoardService {
    constructor(private apiService: ApiService) { }

    getBoardById(id: string): Observable<BoardEntity> {
        return this.apiService.get<BoardEntity>(`board/${id}`)
    }

    getColumnsByBoardId(boardId: string): Observable<ColumnEntity[]> {
        return this.apiService.get<ColumnEntity[]>(`column/get-all/${boardId}`)
    }
}