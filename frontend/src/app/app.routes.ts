import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Registration } from './features/auth/registration/registration';

export const routes: Routes = [
    {
        path: 'signin',
        component: Login
    },
    {
        path: 'signup',
        component: Registration
    }
];
