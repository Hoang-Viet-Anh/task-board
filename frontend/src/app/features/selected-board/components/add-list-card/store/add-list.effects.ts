import { inject, Injectable } from "@angular/core";
import { AddListService } from "../services/add-list.service";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { addListRequest, addListRequestFailure, addListRequestSuccess } from "./add-list.actions";
import { catchError, mergeMap, of } from "rxjs";
import { MessageService } from "primeng/api";
import { getColumnsByBoardId } from "@app/features/selected-board/store/selected-board.actions";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable()
export class AddListEffects {
    private addListService = inject(AddListService)
    private actions$ = inject(Actions)
    private messageService = inject(MessageService)

    addList$ = createEffect(() =>
        this.actions$.pipe(
            ofType(addListRequest),
            mergeMap((action) =>
                this.addListService.addListRequest(action).pipe(
                    mergeMap(() => {
                        this.messageService.add({
                            summary: "List created",
                            severity: "success"
                        });
                        return of(addListRequestSuccess(), getColumnsByBoardId({ id: action.boardId! }))
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.add({
                            summary: "Failed to create list",
                            severity: "error"
                        });
                        return of(addListRequestFailure({ error: 'something went wrong' }))
                    }),
                )
            )
        )
    )
}