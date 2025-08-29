import { createAction } from "@ngrx/store";

export const logoutAction = createAction("Logout user");
export const logoutResponse = createAction("Logout response");