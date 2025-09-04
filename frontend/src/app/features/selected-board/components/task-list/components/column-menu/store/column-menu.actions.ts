import { ColumnEntity } from "@app/features/selected-board/models/column.model";
import { createAction, props } from "@ngrx/store";

export const updateColumnRequest = createAction("Update column", props<ColumnEntity>())
export const updateColumnSuccess = createAction("Update column success")
export const updateColumnFailure = createAction("Update column failure", props<{ error: string }>())

export const removeColumnRequest = createAction("Remove column", props<ColumnEntity>())
export const removeColumnSuccess = createAction("Remove column success")
export const removeColumnFailure = createAction("Remove column failure", props<{ error: string }>())