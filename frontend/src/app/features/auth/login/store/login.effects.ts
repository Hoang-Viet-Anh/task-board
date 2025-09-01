import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { performLogin, performLoginFailure, performLoginSuccess } from "./login.actions";
import { catchError, map, mergeMap, of, tap } from "rxjs";
import { LoginService } from "../services/login.service";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";
import { MessageService } from "primeng/api";

@Injectable()
export class LoginEffects {
    private $actions = inject(Actions)
    private loginService = inject(LoginService)
    private router = inject(Router)
    private messageService = inject(MessageService)

    performLogin$ = createEffect(() =>
        this.$actions.pipe(
            ofType(performLogin),
            mergeMap(action =>
                this.loginService.performLogin(action).pipe(
                    map(() => {
                        this.messageService.add({
                            summary: "You’ve signed in successfully",
                            severity: "success"
                        })

                        return performLoginSuccess();
                    }),
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 401) {
                            this.messageService.add({
                                summary: "Invalid credentials",
                                severity: "error"
                            })
                            return of(performLoginFailure({ error: "invalid credentials" }))
                        }


                        this.messageService.add({
                            summary: "Failed to sign in",
                            severity: "error"
                        })
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