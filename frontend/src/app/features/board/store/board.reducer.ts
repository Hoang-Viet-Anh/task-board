import { RequestStatus } from "@app/shared/models/request.status";
import { BoardEntity } from "../models/board.model";
import { createReducer, on } from "@ngrx/store";
import { getBoards, getBoardsFailure, getBoardsSuccess, removeBoard, removeBoardFailure, removeBoardSuccess, updateBoard, updateBoardFailure, updateBoardSuccess } from "./board.actions";

export interface BoardState {
    getBoardsStatus: RequestStatus,
    updateBoardStatus: RequestStatus,
    removeBoardStatus: RequestStatus,
    boards: BoardEntity[]
}

export const initialBoardState: BoardState = {
    getBoardsStatus: {
        isLoading: false,
        isSuccess: false
    },
    updateBoardStatus: {
        isLoading: false,
        isSuccess: false
    },
    removeBoardStatus: {
        isLoading: false,
        isSuccess: false
    },
    boards: []
}

export const boardReducer = createReducer(
    initialBoardState,
    on(getBoards, state => ({
        ...state,
        getBoardsStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(getBoardsSuccess, (state, actions) => ({
        ...state,
        getBoardsStatus: {
            isLoading: false,
            isSuccess: true
        },
        boards: actions.boards
    })),
    on(getBoardsFailure, (state, actions) => ({
        ...state,
        getBoardsStatus: {
            isLoading: false,
            isSuccess: false,
            error: actions.error
        }
    })),

    on(updateBoard, state => ({
        ...state,
        updateBoardStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(updateBoardSuccess, state => ({
        ...state,
        updateBoardStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(updateBoardFailure, (state, actions) => ({
        ...state,
        updateBoardStatus: {
            isLoading: false,
            isSuccess: false,
            error: actions.error
        }
    })),

    on(removeBoard, state => ({
        ...state,
        removeBoardStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(removeBoardSuccess, state => ({
        ...state,
        removeBoardStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(removeBoardFailure, (state, actions) => ({
        ...state,
        removeBoardStatus: {
            isLoading: false,
            isSuccess: false,
            error: actions.error
        }
    })),
)