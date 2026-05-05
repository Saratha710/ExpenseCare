
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const userGuard: CanActivateFn = () => {
  const auth   = inject(AuthService);
  const router = inject(Router);

  if (auth.isLoggedIn() && auth.isUser) return true;

  // if logged in but wrong role — send to their home
  if (auth.isLoggedIn()) {
    router.navigate(['/home']);
    return false;
  }

  router.navigate(['/user-login']);
  return false;
};