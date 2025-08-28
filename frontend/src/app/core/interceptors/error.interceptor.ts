import { inject, Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
//   private readonly authService = inject(AuthService);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
        //   if (req.url.includes('/protocol/openid-connect/token')) {
        //     return throwError(() => error);
        //   }
        //   return this.handleUnauthorized(req, next, error);
        }
        return throwError(() => error);
      }),
    );
  }

//   private handleUnauthorized(
//     req: HttpRequest<any>,
//     next: HttpHandler,
//     error: HttpErrorResponse,
//   ): Observable<HttpEvent<any>> {
//     // Limit to 1 refresh attempt per request
//     const refreshAttempt = req.headers.get('x-refresh-attempt');
//     if (refreshAttempt === '1') {
//       this.authService.clearTokens();
//       return throwError(() => error);
//     }
//     const refreshToken = localStorage.getItem('refresh_token');
//     if (refreshToken) {
//       return this.authService.refreshAccessToken(refreshToken).pipe(
//         switchMap((tokenResponse: TokenResponse | null) => {
//           if (tokenResponse && tokenResponse.access_token && tokenResponse.refresh_token) {
//             const newAccessToken = tokenResponse.access_token;
//             const newRefreshToken = tokenResponse.refresh_token;
//             localStorage.setItem('refresh_token', newRefreshToken);
//             localStorage.setItem('access_token', newAccessToken);
//             const authReq = req.clone({
//               setHeaders: {
//                 Authorization: `Bearer ${newAccessToken}`,
//                 'x-refresh-attempt': '1',
//               },
//             });
//             return next.handle(authReq);
//           } else {
//             this.logger.error('Invalid token response structure');
//             this.authService.clearTokens();
//             return throwError(() => error);
//           }
//         }),
//         catchError(refreshError => {
//           this.logger.error('Token refresh failed:', refreshError);
//           this.authService.clearTokens();
//           return throwError(() => error);
//         }),
//       );
//     } else {
//       this.authService.clearTokens();
//       return throwError(() => error);
//     }
//   }
}
