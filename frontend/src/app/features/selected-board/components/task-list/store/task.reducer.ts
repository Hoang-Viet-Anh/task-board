import { RequestStatus } from "@app/shared/models/request.status";
import { createReducer, on } from "@ngrx/store";
import { createTaskFailure, createTaskRequest, createTaskSuccess, deleteTaskFailure, deleteTaskRequest, deleteTaskSuccess, updateTaskFailure, updateTaskRequest, updateTaskSuccess } from "./task.actions";

export interface TaskState {
    createStatus: RequestStatus,
    updateStatus: RequestStatus,
    deleteStatus: RequestStatus
}

export const initialTaskState: TaskState = {
    createStatus: {
        isLoading: false,
        isSuccess: false
    },
    updateStatus: {
        isLoading: false,
        isSuccess: false
    },
    deleteStatus: {
        isLoading: false,
        isSuccess: false
    }
}

export const taskReducer = createReducer(
    initialTaskState,

    on(createTaskRequest, state => ({
        ...state,
        createStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(createTaskSuccess, state => ({
        ...state,
        createStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(createTaskFailure, (state, action) => ({
        ...state,
        createStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),

    on(updateTaskRequest, state => ({
        ...state,
        updateStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(updateTaskSuccess, state => ({
        ...state,
        updateStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(updateTaskFailure, (state, action) => ({
        ...state,
        updateStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),

    on(deleteTaskRequest, state => ({
        ...state,
        deleteStatus: {
            isLoading: true,
            isSuccess: false
        }
    })),
    on(deleteTaskSuccess, state => ({
        ...state,
        deleteStatus: {
            isLoading: false,
            isSuccess: true
        }
    })),
    on(deleteTaskFailure, (state, action) => ({
        ...state,
        deleteStatus: {
            isLoading: false,
            isSuccess: false,
            error: action.error
        }
    })),
)