import { createFeatureSelector, createSelector } from "@ngrx/store"
import { RegisterState } from "./register.reducer";

export const registerFeatureKey = 'register'

export const selectRegisterState = createFeatureSelector<RegisterState>(registerFeatureKey);

export const selectRegisterStatus = createSelector(selectRegisterState, (state: RegisterState) => state?.requestStatus);