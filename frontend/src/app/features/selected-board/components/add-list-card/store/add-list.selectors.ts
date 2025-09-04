import { createFeatureSelector, createSelector } from "@ngrx/store"
import { AddListState } from "./add-list.reducer"

export const addListFeatureKey = 'add-list'

export const selectAddListState = createFeatureSelector<AddListState>(addListFeatureKey)

export const selectAddListStatus = createSelector(selectAddListState, state => state.addListStatus)