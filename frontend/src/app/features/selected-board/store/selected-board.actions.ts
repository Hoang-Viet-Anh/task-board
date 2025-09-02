import { BoardEntity } from "@app/features/board/models/board.model";
import { createAction, props } from "@ngrx/store";
import { ColumnEntity } from "../models/column.model";

export const getBoardById = createAction("Get board by id", props<{ id: string }>())
export const getBoardByIdSuccess = createAction("Get board by id success", props<BoardEntity>())
export const getBoardByIdFailure = createAction("Get board by id failure", props<{ error: string }>())

export const getColumnsByBoardId = createAction("Get columns by board id", props<{ id: string }>())
export const getColumnsByBoardIdSuccess = createAction("Get columns by board id success", props<{ columns: ColumnEntity[] }>())
export const getColumnsByBoardIdFailure = createAction("Get columns by board id failure", props<{ error: string }>())

export const clearBoard = createAction("Clear board and column")