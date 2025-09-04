import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { HeaderService } from "../services/header.service";
import { Router } from "@angular/router";
import { logoutAction, logoutResponse } from "./header.actions";
import { catchError, map, mergeMap, of, tap } from "rxjs";

@Injectable()
export class HeaderEffects {
    private $actions = inject(Actions)
    private headerService = inject(HeaderService)
    private route = inject(Router)

    logoutEffect$ = createEffect(() =>
        this.$actions.pipe(
            ofType(logoutAction),
            mergeMap(() =>
                this.headerService.logout().pipe(
                    map(() => logoutResponse()),
                    catchError(() => of(logoutResponse()))
                ),
            )))

    logoutResponse$ = createEffect(() =>
        this.$actions.pipe(
            ofType(logoutResponse),
            tap(() => this.route.navigate(['/signin']))
        ), { dispatch: false })
}