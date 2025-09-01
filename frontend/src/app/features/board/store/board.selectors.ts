import { createFeatureSelector, createSelector } from "@ngrx/store"
import { BoardState } from "./board.reducer"

export const boardFeatureKey = 'board'

export const selectBoardState = createFeatureSelector<BoardState>(boardFeatureKey)

export const selectGetBoardStatus = createSelector(selectBoardState, (state: BoardState) => state.getBoardsStatus)
export const selectUpdateBoardStatus = createSelector(selectBoardState, (state: BoardState) => state.updateBoardStatus)
export const selectRemoveBoardStatus = createSelector(selectBoardState, (state: BoardState) => state.removeBoardStatus)

export const selectBoards = createSelector(selectBoardState, (state: BoardState) => state.boards)