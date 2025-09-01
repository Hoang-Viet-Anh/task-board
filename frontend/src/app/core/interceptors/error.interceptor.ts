import { inject, Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs'; import { AuthService } from '../services/auth.service';
;

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private authService = inject(AuthService)

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          if (req.url.includes('/api/auth/refresh-tokens')) {
            return throwError(() => error);
          }
          return this.handleUnauthorized(req, next, error);
        }
        return throwError(() => error);
      }),
    );
  }

  private handleUnauthorized(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse,
  ): Observable<HttpEvent<any>> {
    // Limit to 1 refresh attempt per request
    const refreshAttempt = req.headers.get('x-refresh-attempt');
    if (refreshAttempt === '1') {
      return throwError(() => error);
    }

    return this.authService.refreshTokens().pipe(
      switchMap(() => {
        const retryReq = req.clone({
          setHeaders: {
            'x-refresh-attempt': '1',
          }
        })

        return next.handle(retryReq)
      }),
      catchError(() => throwError(() => error))
    )

  }
}
