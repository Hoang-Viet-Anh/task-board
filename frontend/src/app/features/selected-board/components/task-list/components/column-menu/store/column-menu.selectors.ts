import { createFeatureSelector, createSelector } from "@ngrx/store"
import { ColumnMenuState } from "./column-menu.reducer"

export const columnMenuFeatureKey = 'column-menu'

export const selectColumnMenuState = createFeatureSelector<ColumnMenuState>(columnMenuFeatureKey)

export const selectUpdateColumnStatus = createSelector(selectColumnMenuState, state => state.updateStatus)
export const selectRemoveColumnStatus = createSelector(selectColumnMenuState, state => state.removeStatus)