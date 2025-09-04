import { createFeatureSelector, createSelector } from "@ngrx/store"
import { TaskState } from "./task.reducer"

export const taskFeatureKey = 'task'

export const selectTaskState = createFeatureSelector<TaskState>(taskFeatureKey)

export const selectCreateTaskStatus = createSelector(selectTaskState, state => state.createStatus)
export const selectUpdateTaskStatus = createSelector(selectTaskState, state => state.updateStatus)
export const selectDeleteTaskStatus = createSelector(selectTaskState, state => state.deleteStatus)