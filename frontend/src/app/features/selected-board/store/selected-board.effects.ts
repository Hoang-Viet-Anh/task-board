import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { SelectedBoardService } from "../services/selected-board.service";
import { getBoardById, getBoardByIdFailure, getBoardByIdSuccess, getColumnsByBoardId, getColumnsByBoardIdFailure, getColumnsByBoardIdSuccess } from "./selected-board.actions";
import { catchError, map, mergeMap, of } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { MessageService } from "primeng/api";
import { Router } from "@angular/router";

@Injectable()
export class SelectedBoardEffects {
    private actions$ = inject(Actions)
    private selectedBoardService = inject(SelectedBoardService)
    private messageService = inject(MessageService)
    private router = inject(Router)

    getBoardById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getBoardById),
            mergeMap((action) =>
                this.selectedBoardService.getBoardById(action.id).pipe(
                    mergeMap((response) => {
                        console.log(response)
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
                    map((response) => {
                        console.log(response)
                        return getColumnsByBoardIdSuccess({ columns: response })
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
}