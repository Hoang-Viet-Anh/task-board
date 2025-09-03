import { inject, Injectable } from "@angular/core";
import { ApiService } from "@app/core/services/api.service";
import { TaskEntity } from "@app/features/selected-board/models/task.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class TaskService {
    private apiService = inject(ApiService)

    createTask(task: TaskEntity): Observable<any> {
        return this.apiService.post<any>('task/create', task)
    }

    removeTask(taskId: string): Observable<any> {
        return this.apiService.delete<any>(`task/delete/${taskId}`)
    }

    updateTask(task: TaskEntity): Observable<any> {
        return this.apiService.post<any>('task/update', task)
    }

    moveTask(task: TaskEntity): Observable<any> {
        return this.apiService.post<any>('task/move', task)
    }
}