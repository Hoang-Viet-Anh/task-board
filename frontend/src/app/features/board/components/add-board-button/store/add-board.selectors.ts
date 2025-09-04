import { createFeatureSelector, createSelector } from "@ngrx/store"
import { AddBoardState } from "./add-board.reducer"

export const addBoardFeatureKey = 'add-board'

export const selectAddBoardState = createFeatureSelector<AddBoardState>(addBoardFeatureKey)

export const selectAddBoardStatus = createSelector(selectAddBoardState, (state: AddBoardState) => state.addBoardStatus)
export const selectJoinBoardStatus = createSelector(selectAddBoardState, (state: AddBoardState) => state.joinBoardStatus)