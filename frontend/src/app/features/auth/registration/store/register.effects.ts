import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of, tap } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { RegisterService } from "../services/register.service";
import { performRegister, performRegisterFailure, performRegisterSuccess } from "./register.actions";
import { Router } from "@angular/router";

@Injectable()
export class RegisterEffects {
    private $actions = inject(Actions)
    private RegisterService = inject(RegisterService)
    private router = inject(Router)

    performRegister$ = createEffect(() =>
        this.$actions.pipe(
            ofType(performRegister),
            mergeMap(action =>
                this.RegisterService.performRegister(action).pipe(
                    map(() => performRegisterSuccess()),
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 409)
                            return of(performRegisterFailure({ error: "username already exists" }))

                        return of(performRegisterFailure({ error: "something went wrong" }))
                    })
                )
            ),
        ))

    performRegisterSuccess$ = createEffect(() =>
        this.$actions.pipe(
            ofType(performRegisterSuccess),
            tap(() => {
                this.router.navigate(["/signin"]);
            })
        ), { dispatch: false })
}