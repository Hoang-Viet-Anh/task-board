import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { createBoard, createBoardFailure, createBoardSuccess, joinBoard, joinBoardFailure, joinBoardSuccess } from "./add-board.actions";

export interface AddBoardState {
    addBoardStatus: RequestStatus;
    joinBoardStatus: RequestStatus;
}

export const initialAddBoardState: AddBoardState = {
    addBoardStatus: {
        isLoading: false,
        isSuccess: false
    },
    joinBoardStatus: {
        isLoading: false,
        isSuccess: false
    }
}

export const addBoardReducer = createReducer(
    initialAddBoardState,
    on(createBoard, state => ({
        ...state,
        addBoardStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(createBoardSuccess, state => ({
        ...state,
        addBoardStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(createBoardFailure, (state, action) => ({
        ...state,
        addBoardStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),

    on(joinBoard, state => ({
        ...state,
        joinBoardStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(joinBoardSuccess, state => ({
        ...state,
        joinBoardStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(joinBoardFailure, (state, action) => ({
        ...state,
        joinBoardStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    }))
)