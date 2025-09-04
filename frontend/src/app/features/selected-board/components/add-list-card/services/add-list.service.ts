import { Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { ColumnEntity } from "@app/features/selected-board/models/column.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AddListService {
    constructor(private apiService: ApiService) { }

    addListRequest(column: ColumnEntity): Observable<any> {
        return this.apiService.post<any>('column/create', column);
    }
}