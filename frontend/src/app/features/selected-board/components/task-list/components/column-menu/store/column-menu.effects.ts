import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { ColumnMenuService } from "../services/column-menu.service";
import { catchError, mergeMap, of } from "rxjs";
import { removeColumnFailure, removeColumnRequest, removeColumnSuccess, updateColumnFailure, updateColumnRequest, updateColumnSuccess } from "./column-menu.actions";
import { MessageService } from "primeng/api";
import { getColumnsByBoardId } from "@app/features/selected-board/store/selected-board.actions";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable()
export class ColumnMenuEffects {
    private actions$ = inject(Actions)
    private columnMenuService = inject(ColumnMenuService)
    private messageService = inject(MessageService)

    updateColumn$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateColumnRequest),
            mergeMap((action) =>
                this.columnMenuService.updateColumnTitle(action).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "List updated",
                            severity: "success"
                        });
                        return of(updateColumnSuccess(), getColumnsByBoardId({ id: action.boardId! }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to update list",
                            severity: "error"
                        });
                        return of(updateColumnFailure({ error: 'something went wrong' }))
                    }),
                )
            )
        )
    )

    removeColumn$ = createEffect(() =>
        this.actions$.pipe(
            ofType(removeColumnRequest),
            mergeMap((action) =>
                this.columnMenuService.removeColumn(action).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "List removed",
                            severity: "success"
                        });
                        return of(removeColumnSuccess(), getColumnsByBoardId({ id: action.boardId! }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to remove list",
                            severity: "error"
                        });
                        return of(removeColumnFailure({ error: 'something went wrong' }))
                    }),
                )
            )
        )
    )
}