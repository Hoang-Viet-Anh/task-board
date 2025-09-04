import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { addListRequest, addListRequestFailure, addListRequestSuccess } from "./add-list.actions";

export interface AddListState {
    addListStatus: RequestStatus
}

export const initialAddListState: AddListState = {
    addListStatus: {
        isLoading: false,
        isSuccess: false
    }
}

export const addListReducer = createReducer(
    initialAddListState,
    on(addListRequest, state => ({
        ...state,
        addListStatus: {
            isLoading: true,
            isSuccess: false,
        }
    })),
    on(addListRequestSuccess, state => ({
        ...state,
        addListStatus: {
            isLoading: false,
            isSuccess: true,
        }
    })),
    on(addListRequestFailure, (state, action) => ({
        ...state,
        addListStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    }))
)

