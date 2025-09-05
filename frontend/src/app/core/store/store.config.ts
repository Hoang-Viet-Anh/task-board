import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { RegisterEffects } from '@app/features/auth/registration/store/register.effects';
import { LoginEffects } from '@app/features/auth/login/store/login.effects';
import { loginFeatureKey } from '@app/features/auth/login/store/login.selectors';
import { loginReducer } from '@app/features/auth/login/store/login.reducer';
import { registerFeatureKey } from '@app/features/auth/registration/store/register.selectors';
import { registerReducer } from '@app/features/auth/registration/store/register.reducer';
import { HeaderEffects } from '@app/layout/header/store/header.effects';
import { addBoardFeatureKey } from '@app/features/board/components/add-board-button/store/add-board.selectors';
import { addBoardReducer } from '@app/features/board/components/add-board-button/store/add-board.reducer';
import { AddBoardEffects } from '@app/features/board/components/add-board-button/store/add-board.effects';
import { BoardEffects } from '@app/features/board/store/board.effects';
import { boardFeatureKey } from '@app/features/board/store/board.selectors';
import { boardReducer } from '@app/features/board/store/board.reducer';
import { selectedBoardFeatureKey } from '@app/features/selected-board/store/selected-board.selectors';
import { selectedBoardReducer } from '@app/features/selected-board/store/selected-board.reducer';
import { SelectedBoardEffects } from '@app/features/selected-board/store/selected-board.effects';
import { AddListEffects } from '@app/features/selected-board/components/add-list-card/store/add-list.effects';
import { addListFeatureKey } from '@app/features/selected-board/components/add-list-card/store/add-list.selectors';
import { addListReducer } from '@app/features/selected-board/components/add-list-card/store/add-list.reducer';
import { ColumnMenuEffects } from '@app/features/selected-board/components/task-list/components/column-menu/store/column-menu.effects';
import { columnMenuFeatureKey } from '@app/features/selected-board/components/task-list/components/column-menu/store/column-menu.selectors';
import { columnMenuReducer } from '@app/features/selected-board/components/task-list/components/column-menu/store/column-menu.reducer';
import { TaskEffects } from '@app/features/selected-board/components/task-list/store/task.effects';
import { taskFeatureKey } from '@app/features/selected-board/components/task-list/store/task.selectors';
import { taskReducer } from '@app/features/selected-board/components/task-list/store/task.reducer';

export const providedStore = provideStore({
    [loginFeatureKey]: loginReducer,
    [registerFeatureKey]: registerReducer,
    [addBoardFeatureKey]: addBoardReducer,
    [boardFeatureKey]: boardReducer,
    [selectedBoardFeatureKey]: selectedBoardReducer,
    [addListFeatureKey]: addListReducer,
    [columnMenuFeatureKey]: columnMenuReducer,
    [taskFeatureKey]: taskReducer
})

export const providedEffects = provideEffects([
    RegisterEffects,
    LoginEffects,
    HeaderEffects,
    AddBoardEffects,
    BoardEffects,
    SelectedBoardEffects,
    AddListEffects,
    ColumnMenuEffects,
    TaskEffects
])