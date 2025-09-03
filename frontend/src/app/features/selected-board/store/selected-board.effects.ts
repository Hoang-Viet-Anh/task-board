import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { SelectedBoardService } from "../services/selected-board.service";
import { getBoardById, getBoardByIdFailure, getBoardByIdSuccess, getColumnsByBoardId, getColumnsByBoardIdFailure, getColumnsByBoardIdSuccess, getLogsByBoardId, getLogsByBoardIdFailure, getLogsByBoardIdSuccess, loadMoreLogs, loadMoreLogsSuccess } from "./selected-board.actions";
import { catchError, map, mergeMap, of, withLatestFrom } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { MessageService } from "primeng/api";
import { Router } from "@angular/router";
import { ColumnEntity } from "../models/column.model";
import { Store } from "@ngrx/store";
import { selectLogsPage } from "./selected-board.selectors";

@Injectable()
export class SelectedBoardEffects {
    private actions$ = inject(Actions)
    private selectedBoardService = inject(SelectedBoardService)
    private messageService = inject(MessageService)
    private router = inject(Router)
    private store = inject(Store)

    getBoardById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getBoardById),
            mergeMap((action) =>
                this.selectedBoardService.getBoardById(action.id).pipe(
                    mergeMap((response) => {
                        return of(getBoardByIdSuccess(response), getColumnsByBoardId({ id: action.id }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        let message = 'Something went wrong'

                        if (error.status === 404) {
                            message = 'Board not found'
                        } else if (error.status === 403) {
                            message = 'You don’t have permission to access this board.'
                        }

                        this.messageService.add({
                            summary: message,
                            severity: 'error'
                        })
                        this.router.navigate([''])
                        return of(getBoardByIdFailure({ error: message }))
                    })
                )
            )
        )
    )

    getColumnsByBoardId$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getColumnsByBoardId),
            mergeMap((action) =>
                this.selectedBoardService.getColumnsByBoardId(action.id).pipe(
                    mergeMap((response) => {
                        const parsedDate: ColumnEntity[] = response.map(column => ({
                            ...column,
                            tasks: column.tasks?.map(t => ({
                                ...t,
                                dueDate: new Date(t.dueDate!)
                            }))
                        }))
                        return of(getColumnsByBoardIdSuccess({ columns: parsedDate }), getLogsByBoardId({ id: action.id }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        let message = 'Something went wrong'

                        if (error.status === 403) {
                            message = 'You don’t have permission to access these columns.'
                        }

                        this.messageService.add({
                            summary: message,
                            severity: 'error'
                        })
                        this.router.navigate([''])
                        return of(getColumnsByBoardIdFailure({ error: message }))
                    })
                )
            )
        )
    )

    getLogsByBoardId$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getLogsByBoardId),
            mergeMap((action) =>
                this.selectedBoardService.getLogsByBoardId(action.id).pipe(
                    map((response) => {
                        return getLogsByBoardIdSuccess({ logs: response })
                    }),
                    catchError((error: HttpErrorResponse) => {
                        let message = 'Something went wrong'

                        this.messageService.add({
                            summary: message,
                            severity: 'error'
                        })
                        return of(getLogsByBoardIdFailure({ error: message }))
                    })
                )
            )
        )
    )

    getPagedLogs$ = createEffect(() =>
        this.actions$.pipe(
            ofType(loadMoreLogs),
            withLatestFrom(this.store.select(selectLogsPage)),
            mergeMap(([action, page]) =>
                this.selectedBoardService.getPagedLogs(action.id, page).pipe(
                    map((response) => {
                        return loadMoreLogsSuccess({ logs: response })
                    }),
                    catchError(() => {
                        let message = 'Something went wrong'

                        this.messageService.add({
                            summary: message,
                            severity: 'error'
                        })
                        return of()
                    })
                )
            )
        )
    )
}