import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of, tap } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { RegisterService } from "../services/register.service";
import { performRegister, performRegisterFailure, performRegisterSuccess } from "./register.actions";
import { Router } from "@angular/router";
import { MessageService } from "primeng/api";

@Injectable()
export class RegisterEffects {
    private $actions = inject(Actions)
    private RegisterService = inject(RegisterService)
    private router = inject(Router)
    private messageService = inject(MessageService)

    performRegister$ = createEffect(() =>
        this.$actions.pipe(
            ofType(performRegister),
            mergeMap(action =>
                this.RegisterService.performRegister(action).pipe(
                    map(() => {
                        this.messageService.add({
                            summary: "Account created",
                            severity: "success"
                        })

                        return performRegisterSuccess();
                    }),
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 409) {
                            this.messageService.add({
                                summary: "Username already exists",
                                severity: "error"
                            })
                            return of(performRegisterFailure({ error: "username already exists" }))
                        }

                        this.messageService.add({
                            summary: "Failed to create account",
                            severity: "error"
                        })
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