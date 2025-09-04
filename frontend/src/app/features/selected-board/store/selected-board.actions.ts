import { BoardEntity } from "@app/features/board/models/board.model";
import { createAction, props } from "@ngrx/store";
import { ColumnEntity } from "../models/column.model";
import { TaskEntity } from "../models/task.model";
import { Log } from "../models/log.model";

export const getBoardById = createAction("Get board by id", props<{ id: string }>())
export const getBoardByIdSuccess = createAction("Get board by id success", props<BoardEntity>())
export const getBoardByIdFailure = createAction("Get board by id failure", props<{ error: string }>())

export const getColumnsByBoardId = createAction("Get columns by board id", props<{ id: string }>())
export const getColumnsByBoardIdSuccess = createAction("Get columns by board id success", props<{ columns: ColumnEntity[] }>())
export const getColumnsByBoardIdFailure = createAction("Get columns by board id failure", props<{ error: string }>())

export const getLogsByBoardId = createAction("Get history by board id", props<{ id: string }>())
export const getLogsByBoardIdSuccess = createAction("Get history by board id success", props<{ logs: Log[] }>())
export const getLogsByBoardIdFailure = createAction("Get history by board id failure", props<{ error: string }>())

export const clearBoard = createAction("Clear board and column")

export const changeTaskList = createAction("Change task list", props<{ task: TaskEntity, previousColumn: ColumnEntity, currentColumn: ColumnEntity }>())
export const changeTaskListSuccess = createAction("Change task list success")
export const changeTaskListFailure = createAction("Change task list failure", props<{ error: string }>())

export const loadMoreLogs = createAction("Load more logs", props<{ id: string }>())
export const loadMoreLogsSuccess = createAction("Load more logs success", props<{ logs: Log[] }>())