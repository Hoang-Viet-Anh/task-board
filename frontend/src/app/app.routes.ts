import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Registration } from './features/auth/registration/registration';
import { Board } from './features/board/board';
import { HeaderLayout } from './layout/header/header';

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
    }
];
