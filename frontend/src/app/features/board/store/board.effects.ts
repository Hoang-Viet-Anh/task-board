import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { BoardService } from "../services/board.service";
import { catchError, map, mergeMap, of } from "rxjs";
import { getBoards, getBoardsFailure, getBoardsSuccess } from "./board.actions";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable()
export class BoardEffects {
    private actions$ = inject(Actions)
    private boardService = inject(BoardService)

    getBoards$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getBoards),
            mergeMap(() =>
                this.boardService.getBoards().pipe(
                    map((response) => getBoardsSuccess({ boards: response })),
                    catchError((error: HttpErrorResponse) => {
                        return of(getBoardsFailure({ error: 'something went wrong' }))
                    })
                )
            )
        )
    )

    
}