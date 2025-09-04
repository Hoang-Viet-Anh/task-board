import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { removeColumnFailure, removeColumnRequest, removeColumnSuccess, updateColumnFailure, updateColumnRequest, updateColumnSuccess } from "./column-menu.actions";

export interface ColumnMenuState {
    updateStatus: RequestStatus,
    removeStatus: RequestStatus,
}

export const initialColumnMenuState: ColumnMenuState = {
    updateStatus: {
        isLoading: false,
        isSuccess: false,
    },
    removeStatus: {
        isLoading: false,
        isSuccess: false,
    }
}

export const columnMenuReducer = createReducer(
    initialColumnMenuState,
    on(updateColumnRequest, state => ({
        ...state,
        updateStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(updateColumnSuccess, state => ({
        ...state,
        updateStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(updateColumnFailure, (state, action) => ({
        ...state,
        updateStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),

    on(removeColumnRequest, state => ({
        ...state,
        removeStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(removeColumnSuccess, state => ({
        ...state,
        removeStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(removeColumnFailure, (state, action) => ({
        ...state,
        removeStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),
)