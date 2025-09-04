import { createAction, props } from "@ngrx/store";
import { RegisterForm } from "../models/register.model";

export const performRegister = createAction('Register user', props<RegisterForm>())
export const performRegisterSuccess = createAction('Register user success')
export const performRegisterFailure = createAction('Register user failure', props<{ error: string }>())

