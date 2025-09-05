import { inject, Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { ColumnEntity } from "@app/features/selected-board/models/column.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class ColumnMenuService {
    private apiService = inject(ApiService)

    updateColumn(column: ColumnEntity): Observable<any> {
        return this.apiService.post<any>('column/update', column)
    }

    removeColumn(column: ColumnEntity): Observable<any> {
        return this.apiService.delete(`column/delete/${column.id}`)
    }
}