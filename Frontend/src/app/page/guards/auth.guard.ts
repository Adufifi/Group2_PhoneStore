import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard = () => {
  const router = inject(Router);

  if (localStorage.getItem('email')) {
    return true;
  }
  router.navigate(['/login']);
  return false;
};
