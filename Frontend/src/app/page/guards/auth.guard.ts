import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard = () => {
  const router = inject(Router);

  if (localStorage.getItem('userEmail')) {
    return true;
  }

  // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
  router.navigate(['/login']);
  return false;
};
