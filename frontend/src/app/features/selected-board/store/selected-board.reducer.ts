import { BoardEntity } from "@app/features/board/models/board.model";
import { ColumnEntity } from "../models/column.model";
import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { getColumnsByBoardId, getColumnsByBoardIdSuccess, getBoardById, getBoardByIdSuccess, getBoardByIdFailure, getColumnsByBoardIdFailure, clearBoard, changeTaskList, getLogsByBoardIdSuccess, loadMoreLogs, loadMoreLogsSuccess } from "./selected-board.actions";
import { Log } from "../models/log.model";

export interface SelectedBoardState {
    boardStatus: RequestStatus,
    columnsStatus: RequestStatus,
    board?: BoardEntity,
    columns: ColumnEntity[],
    logs: Log[],
    logPage: number
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
    columns: [],
    logs: [],
    logPage: 0
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

    on(changeTaskList, (state, action) => {
        return ({
            ...state,
            columns: state.columns.map(c => {
                const oldTasks = c.tasks ?? []

                const filteredTasks = c.id === action.task.columnId
                    ? oldTasks.filter(t => t.id !== action.task.id)
                    : oldTasks;

                const updatedTasks = c.id === action.newColumn.id
                    ? [...filteredTasks, { ...action.task, columnId: action.newColumn.id }].sort(
                        (a, b) => a.dueDate!.getTime() - b.dueDate!.getTime()
                    )
                    : filteredTasks;


                return ({
                    ...c,
                    tasks: updatedTasks
                })
            })
        })
    }),

    on(getLogsByBoardIdSuccess, (state, action) => ({
        ...state,
        logs: action.logs
    })),

    on(loadMoreLogs, state => ({
        ...state,
        logPage: state.logPage + 1
    })),
    on(loadMoreLogsSuccess, (state, action) => ({
        ...state,
        logs: [...state.logs, ...action.logs]
    })),

    on(clearBoard, state => initialSelectedBoardState)
)