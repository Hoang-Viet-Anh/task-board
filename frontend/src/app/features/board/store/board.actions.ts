import { createAction, props } from "@ngrx/store";
import { BoardEntity } from "../models/board.model";

export const getBoards = createAction('Get boards')
export const getBoardsSuccess = createAction('Get boards success', props<{ boards: BoardEntity[] }>())
export const getBoardsFailure = createAction('Get boards failure', props<{ error: string }>())

export const updateBoard = createAction('Update board', props<BoardEntity>())
export const updateBoardSuccess = createAction('Update board success')
export const updateBoardFailure = createAction('Update board failure', props<{ error: string }>())

export const removeBoard = createAction('Remove board', props<{ id: string }>())
export const removeBoardSuccess = createAction('Remove board success')
export const removeBoardFailure = createAction('Remove board failure', props<{ error: string }>())

