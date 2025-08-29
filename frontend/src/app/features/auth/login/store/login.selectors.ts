import { createFeatureSelector, createSelector } from "@ngrx/store"
import { LoginState } from "./login.reducer"

export const loginFeatureKey = 'login'

export const selectLoginState = createFeatureSelector<LoginState>(loginFeatureKey);

export const selectLoginStatus = createSelector(selectLoginState, (state: LoginState) => state?.requestStatus);