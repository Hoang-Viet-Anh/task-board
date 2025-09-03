import { TaskEntity } from "@app/features/selected-board/models/task.model";
import { createAction, props } from "@ngrx/store";

export const createTaskRequest = createAction('Create task', props<{ task: TaskEntity, boardId: string }>())
export const createTaskSuccess = createAction('Create task success')
export const createTaskFailure = createAction('Create task failure', props<{ error: string }>())

export const updateTaskRequest = createAction('Update task', props<{ task: TaskEntity, boardId: string }>())
export const updateTaskSuccess = createAction('Update task success')
export const updateTaskFailure = createAction('Update task failure', props<{ error: string }>())

export const deleteTaskRequest = createAction('Delete task', props<{ taskId: string, boardId: string }>())
export const deleteTaskSuccess = createAction('Delete task success')
export const deleteTaskFailure = createAction('Delete task failure', props<{ error: string }>())