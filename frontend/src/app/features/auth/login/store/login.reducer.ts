import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { performLogin, performLoginFailure, performLoginSuccess } from "./login.actions";

export interface LoginState {
    requestStatus: RequestStatus
}

export const initialLoginState: LoginState = {
    requestStatus: {
        isLoading: false,
        isSuccess: false,
    }
}

export const loginReducer = createReducer(
    initialLoginState,
    on(performLogin, () => ({ requestStatus: { isLoading: true, isSuccess: false } })),
    on(performLoginSuccess, () => ({ requestStatus: { isLoading: false, isSuccess: true } })),
    on(performLoginFailure, (_state, action) => ({ requestStatus: { isLoading: false, isSuccess: false, error: action.error } }))
) 