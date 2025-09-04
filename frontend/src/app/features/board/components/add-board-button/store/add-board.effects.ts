import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { AddBoardService } from "../services/add-board.service";
import { createBoard, createBoardFailure, createBoardSuccess, joinBoard, joinBoardFailure, joinBoardSuccess } from "./add-board.actions";
import { catchError, map, mergeMap, of, tap } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { MessageService } from "primeng/api";
import { getBoards } from "@app/features/board/store/board.actions";

@Injectable()
export class AddBoardEffects {
    private actions$ = inject(Actions)
    private addBoardService = inject(AddBoardService)
    private messageService = inject(MessageService)

    createBoard$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createBoard),
            mergeMap(action =>
                this.addBoardService.createBoard(action.boardTitle).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Board created",
                            severity: "success"
                        });

                        return of(createBoardSuccess(), getBoards())
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to create board",
                            severity: "error"
                        });

                        return of(createBoardFailure({ error: 'something went wrong' }))
                    })
                )
            ),
        )
    )

    joinBoard$ = createEffect(() =>
        this.actions$.pipe(
            ofType(joinBoard),
            mergeMap(action =>
                this.addBoardService.joinBoard(action.inviteCode).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "Board joined",
                            severity: "success"
                        });

                        return of(joinBoardSuccess(), getBoards())
                    }),
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 404) {
                            this.messageService.add({
                                summary: "Board not found",
                                severity: "error"
                            });
                            return of(joinBoardFailure({ error: 'board not found' }))
                        }

                        this.messageService.add({
                            summary: "Failed to join board",
                            severity: "error"
                        });
                        return of(joinBoardFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )
}