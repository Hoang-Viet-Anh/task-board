import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { providedEffects, providedStore } from './store/store.config';

export const CORE_PROVIDERS = [
  provideHttpClient(withInterceptorsFromDi()),
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true,
  },
  providedEffects,
  providedStore
];
