import { inject, Injectable } from "@angular/core";
import { TaskService } from "../services/task.service";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { MessageService } from "primeng/api";
import { catchError, mergeMap, of } from "rxjs";
import { createTaskFailure, createTaskRequest, createTaskSuccess, deleteTaskFailure, deleteTaskRequest, deleteTaskSuccess, updateTaskFailure, updateTaskRequest, updateTaskSuccess } from "./task.actions";
import { HttpErrorResponse } from "@angular/common/http";
import { changeTaskList, changeTaskListFailure, changeTaskListSuccess, getColumnsByBoardId } from "@app/features/selected-board/store/selected-board.actions";
import { DialogService } from "@app/shared/services/dialog.service";

@Injectable()
export class TaskEffects {
    private taskService = inject(TaskService)
    private actions$ = inject(Actions)
    private messageService = inject(MessageService)
    private dialogService = inject(DialogService)

    createTask$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createTaskRequest),
            mergeMap((action) =>
                this.taskService.createTask(action.task).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Task created",
                            severity: "success"
                        });
                        this.dialogService.close()
                        return of(createTaskSuccess(), getColumnsByBoardId({ id: action.boardId }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to create task",
                            severity: "error"
                        });
                        return of(createTaskFailure({ error: 'something went wrong' }))
                    }),
                )
            )
        )
    )

    updateTask$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateTaskRequest),
            mergeMap((action) =>
                this.taskService.updateTask(action.task).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Task updated",
                            severity: "success"
                        });
                        this.dialogService.close()
                        return of(updateTaskSuccess(), getColumnsByBoardId({ id: action.boardId }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to update task",
                            severity: "error"
                        });
                        return of(updateTaskFailure({ error: 'something went wrong' }))
                    }),
                )
            )
        )
    )

    deleteTask$ = createEffect(() =>
        this.actions$.pipe(
            ofType(deleteTaskRequest),
            mergeMap((action) =>
                this.taskService.removeTask(action.taskId).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Task removed",
                            severity: "success"
                        });
                        this.dialogService.close()
                        return of(deleteTaskSuccess(), getColumnsByBoardId({ id: action.boardId }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to remove task",
                            severity: "error"
                        });
                        return of(deleteTaskFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )

    moveTask$ = createEffect(() =>
        this.actions$.pipe(
            ofType(changeTaskList),
            mergeMap((action) =>
                this.taskService.moveTask({
                    id: action.task.id,
                    columnId: action.newColumn.id
                }).pipe(
                    mergeMap(() => {
                        return of(changeTaskListSuccess(), getColumnsByBoardId({ id: action.newColumn.boardId! }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to update task",
                            severity: "error"
                        });
                        return of(changeTaskListFailure({ error: 'something went wrong' }), getColumnsByBoardId({ id: action.newColumn.boardId! }))
                    }),
                )
            )
        )
    )
}