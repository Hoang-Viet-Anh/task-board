import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { performLogin, performLoginFailure, performLoginSuccess } from "./login.actions";
import { catchError, map, mergeMap, of, tap } from "rxjs";
import { LoginService } from "../services/login.service";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable()
export class LoginEffects {
    private $actions = inject(Actions)
    private loginService = inject(LoginService)
    private router = inject(Router)

    performLogin$ = createEffect(() =>
        this.$actions.pipe(
            ofType(performLogin),
            mergeMap(action =>
                this.loginService.performLogin(action).pipe(
                    map(() => performLoginSuccess()),
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 401)
                            return of(performLoginFailure({ error: "invalid credentials" }))

                        return of(performLoginFailure({ error: "something went wrong" }))
                    })
                )
            ),
        ))

    performLoginSuccess$ = createEffect(() =>
        this.$actions.pipe(
            ofType(performLoginSuccess),
            tap(() => this.router.navigate(['']))
        ), { dispatch: false })
}