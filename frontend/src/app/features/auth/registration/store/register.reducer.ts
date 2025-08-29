import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { performRegister, performRegisterFailure, performRegisterSuccess } from "./register.actions";

export interface RegisterState {
    requestStatus: RequestStatus
}

export const initialRegisterState: RegisterState = {
    requestStatus: {
        isLoading: false,
        isSuccess: false,
    }
}

export const registerReducer = createReducer(
    initialRegisterState,
    on(performRegister, () => ({ requestStatus: { isLoading: true, isSuccess: false } })),
    on(performRegisterSuccess, () => ({ requestStatus: { isLoading: false, isSuccess: true } })),
    on(performRegisterFailure, (_state, action) => ({ requestStatus: { isLoading: false, isSuccess: false, error: action.error } }))
)