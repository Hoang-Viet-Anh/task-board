import { createAction, props } from "@ngrx/store";

export const createBoard = createAction('Create board', props<{ boardTitle: string }>())
export const createBoardSuccess = createAction('Create board success')
export const createBoardFailure = createAction('Create board failure', props<{ error: string }>())

export const joinBoard = createAction('Join board', props<{ inviteCode: string }>())
export const joinBoardSuccess = createAction('Join board success')
export const joinBoardFailure = createAction('Join board failure', props<{ error: string }>())