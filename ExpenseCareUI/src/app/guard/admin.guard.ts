import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const adminGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  if (auth.isAdmin) return true;

  if(auth.isTrustee) {
    router.navigate(['/home']);
    return false;
  }

    if (auth.isUser) {
    router.navigate(['/user-home']);
    return false;
  }

  router.navigate(['/home']);
  return false;
};