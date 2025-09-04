import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Registration } from './features/auth/registration/registration';
import { HeaderLayout } from './layout/header/header';
import { Board } from './features/board/board';
import { SelectedBoard } from './features/selected-board/selected-board';

export const routes: Routes = [
    {
        path: 'signin',
        component: Login
    },
    {
        path: 'signup',
        component: Registration
    },
    {
        path: '',
        component: HeaderLayout,
        children: [
            {
                path: '',
                component: Board
            }
        ]
    },
    {
        path: 'board/:id',
        component: HeaderLayout,
        children: [
            {
                path: '',
                component: SelectedBoard
            }
        ]
    }
];
