import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { BoardService } from "../services/board.service";
import { catchError, map, mergeMap, of } from "rxjs";
import { getBoards, getBoardsFailure, getBoardsSuccess, removeBoard, removeBoardFailure, removeBoardSuccess, updateBoard, updateBoardFailure, updateBoardSuccess } from "./board.actions";
import { HttpErrorResponse } from "@angular/common/http";
import { MessageService } from "primeng/api";
import { DialogService } from "@app/shared/services/dialog.service";

@Injectable()
export class BoardEffects {
    private actions$ = inject(Actions)
    private boardService = inject(BoardService)
    private messageService = inject(MessageService)
    private dialogService = inject(DialogService)

    getBoards$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getBoards),
            mergeMap(() =>
                this.boardService.getBoards().pipe(
                    map((response) => {
                        return getBoardsSuccess({ boards: response })
                    }),
                    catchError((error: HttpErrorResponse) => {
                        return of(getBoardsFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )

    updateBoards$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateBoard),
            mergeMap((action) =>
                this.boardService.updateBoard(action).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Board updated",
                            severity: "success"
                        });
                        this.dialogService.close()
                        return of(updateBoardSuccess(), getBoards())
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to update board",
                            severity: "error"
                        });
                        return of(updateBoardFailure({ error: 'something went wrong' }))
                    }),
                )
            )
        )
    )

    removeBoards$ = createEffect(() =>
        this.actions$.pipe(
            ofType(removeBoard),
            mergeMap((action) =>
                this.boardService.removeBoard(action.id).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Board removed",
                            severity: "success"
                        });
                        this.dialogService.close()
                        return of(removeBoardSuccess(), getBoards())
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to remove board",
                            severity: "error"
                        });
                        return of(removeBoardFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )


}