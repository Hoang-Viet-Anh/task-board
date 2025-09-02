import { BoardEntity } from "@app/features/board/models/board.model";
import { ColumnEntity } from "../models/column.model";
import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { getColumnsByBoardId, getColumnsByBoardIdSuccess, getBoardById, getBoardByIdSuccess, getBoardByIdFailure, getColumnsByBoardIdFailure, clearBoard } from "./selected-board.actions";

export interface SelectedBoardState {
    boardStatus: RequestStatus,
    columnsStatus: RequestStatus,
    board?: BoardEntity,
    columns: ColumnEntity[]
}

export const initialSelectedBoardState: SelectedBoardState = {
    boardStatus: {
        isLoading: true,
        isSuccess: false
    },
    columnsStatus: {
        isLoading: true,
        isSuccess: false
    },
    columns: []
}

export const selectedBoardReducer = createReducer(
    initialSelectedBoardState,
    on(getBoardById, state => ({
        ...state,
        boardStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(getBoardByIdSuccess, (state, action) => ({
        ...state,
        boardStatus: {
            isLoading: false,
            isSuccess: true
        },
        board: action
    })),
    on(getBoardByIdFailure, (state, action) => ({
        ...state,
        boardStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),

    on(getColumnsByBoardId, state => ({
        ...state,
        columnsStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(getColumnsByBoardIdSuccess, (state, action) => ({
        ...state,
        columnsStatus: {
            isLoading: false,
            isSuccess: true
        },
        columns: action.columns
    })),
    on(getColumnsByBoardIdFailure, (state, action) => ({
        ...state,
        columnsStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),

    on(clearBoard, state => initialSelectedBoardState)
)