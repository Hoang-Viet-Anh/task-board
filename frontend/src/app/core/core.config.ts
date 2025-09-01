import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { RegisterEffects } from '@app/features/auth/registration/store/register.effects';
import { LoginEffects } from '@app/features/auth/login/store/login.effects';
import { loginFeatureKey } from '@app/features/auth/login/store/login.selectors';
import { loginReducer } from '@app/features/auth/login/store/login.reducer';
import { registerFeatureKey } from '@app/features/auth/registration/store/register.selectors';
import { registerReducer } from '@app/features/auth/registration/store/register.reducer';
import { HeaderEffect } from '@app/layout/header/store/header.effects';
import { addBoardFeatureKey } from '@app/features/board/components/add-board-button/store/add-board.selectors';
import { addBoardReducer } from '@app/features/board/components/add-board-button/store/add-board.reducer';
import { AddBoardEffects } from '@app/features/board/components/add-board-button/store/add-board.effects';
import { BoardEffects } from '@app/features/board/store/board.effects';
import { boardFeatureKey } from '@app/features/board/store/board.selectors';
import { boardReducer } from '@app/features/board/store/board.reducer';

export const CORE_PROVIDERS = [
  provideHttpClient(withInterceptorsFromDi()),
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true,
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
  },
  provideEffects([RegisterEffects, LoginEffects, HeaderEffect, AddBoardEffects, BoardEffects]),
  provideStore({
    [loginFeatureKey]: loginReducer,
    [registerFeatureKey]: registerReducer,
    [addBoardFeatureKey]: addBoardReducer,
    [boardFeatureKey]: boardReducer
  })
];
