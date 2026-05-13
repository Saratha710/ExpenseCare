import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const authGuard: CanActivateFn = () => {
  const auth   = inject(AuthService);
  const router = inject(Router);

  // not logged in → landing
  if (!auth.isLoggedIn()) {
    router.navigate(['']);
    return false;
  }

  // user trying to access admin/trustee pages → user home
  if (auth.isUser) {
    router.navigate(['/user-home']);
    return false;
  }

  //Admin or Trustee allow
  return true;
};