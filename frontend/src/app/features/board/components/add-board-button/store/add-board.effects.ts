import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { AddBoardService } from "../services/add-board.service";
import { createBoard, createBoardFailure, createBoardSuccess, joinBoard, joinBoardFailure, joinBoardSuccess } from "./add-board.actions";
import { catchError, map, mergeMap, of } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable()
export class AddBoardEffects {
    private actions$ = inject(Actions)
    private addBoardService = inject(AddBoardService)

    createBoard$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createBoard),
            mergeMap(action =>
                this.addBoardService.createBoard(action.boardTitle).pipe(
                    map(() => createBoardSuccess()),
                    catchError((error: HttpErrorResponse) => {
                        return of(createBoardFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )

    joinBoard$ = createEffect(() =>
        this.actions$.pipe(
            ofType(joinBoard),
            mergeMap(action =>
                this.addBoardService.joinBoard(action.inviteCode).pipe(
                    map(() => joinBoardSuccess()),
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 404)
                            return of(joinBoardFailure({ error: 'board not found' }))

                        return of(joinBoardFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )
}