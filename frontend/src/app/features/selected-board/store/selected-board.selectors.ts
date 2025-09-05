import { createFeatureSelector, createSelector } from "@ngrx/store"
import { SelectedBoardState } from "./selected-board.reducer"

export const selectedBoardFeatureKey = "selected-board"

export const selectSelectedBoardState = createFeatureSelector<SelectedBoardState>(selectedBoardFeatureKey)

export const selectGetBoardByIdStatus = createSelector(selectSelectedBoardState, state => state.boardStatus)
export const selectGetColumnsByBoardIdStatus = createSelector(selectSelectedBoardState, state => state.columnsStatus)

export const selectSelectedBoard = createSelector(selectSelectedBoardState, state => state.board)
export const selectSelectedBoardColumns = createSelector(selectSelectedBoardState, state => state.columns)
export const selectSelectedBoardLogs = createSelector(selectSelectedBoardState, state => state.logs)
export const selectLogsPage = createSelector(selectSelectedBoardState, state => state.logPage)
export const selectBoardMembers = createSelector(selectSelectedBoardState, state => state.board?.members)

export const selectTaskById = (taskId: string) => createSelector(selectSelectedBoardColumns, state => state.flatMap(col => col.tasks).find(t => t?.id === taskId))
export const selectColumnById = (columnId: string) => createSelector(selectSelectedBoardColumns, state => state.find(c => c.id === columnId))