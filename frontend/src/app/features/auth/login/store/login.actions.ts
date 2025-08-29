import { createAction, props } from "@ngrx/store";
import { LoginForm } from "../models/login.model";

export const performLogin = createAction('Authenticate user', props<LoginForm>())
export const performLoginSuccess = createAction('Authenticate user success')
export const performLoginFailure = createAction('Authenticate user failure', props<{ error: string }>())

