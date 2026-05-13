import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // Skip adding token for auth endpoints
  const isAuthEndpoint = req.url.includes(`${environment.apiUrl}/api/auth/`);
  const token = auth.accessToken;

  // Clone request and add Authorization header if token exists
  const authReq = (token && !isAuthEndpoint)
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // If 401 and we have a refresh token — try to refresh
      if (error.status === 401 && auth.refreshToken && !isAuthEndpoint) {
        return auth.refreshAccessToken().pipe(
          switchMap((res) => {
            // Retry original request with new token
            const retryReq = req.clone({
              setHeaders: { Authorization: `Bearer ${res.accessToken}` }
            });
            return next(retryReq);
          }),
          catchError((refreshError) => {
            // Refresh failed — force logout
            const role= auth.role;
            auth.clearSession();
            if(role === 'User') {
                router.navigate(['/user-login']);
            }
            else{
                router.navigate(['/login']);
            }


            return throwError(() => refreshError);
          })
        );
      }

      if (error.status === 403) {
        router.navigate(['/']);
      }


      return throwError(() => error);
    })
  );
};