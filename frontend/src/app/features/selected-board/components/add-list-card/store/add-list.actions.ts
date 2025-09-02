import { ColumnEntity } from "@app/features/selected-board/models/column.model";
import { createAction, props } from "@ngrx/store";

export const addListRequest = createAction('Add list request', props<ColumnEntity>())
export const addListRequestSuccess = createAction('Add list request success')
export const addListRequestFailure = createAction('Add list request failure', props<{ error: string }>())