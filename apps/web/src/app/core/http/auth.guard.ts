import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

function storedUser(): { email?: string; displayName?: string; roles?: string[] } | null {
  const raw = localStorage.getItem('madai.user') || sessionStorage.getItem('madai.user');
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

export function isSuperAdminUser(): boolean {
  const user = storedUser();
  const evidence = [user?.email, user?.displayName, ...(user?.roles ?? [])].join(' ').toLowerCase();
  return evidence.includes('admin@madprospects.com') || evidence.includes('systemadmin') || evidence.includes('superadmin');
}

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  return localStorage.getItem('madai.accessToken') || sessionStorage.getItem('madai.accessToken')
    ? true
    : router.createUrlTree(['/login']);
};

export const superAdminGuard: CanActivateFn = () => {
  const router = inject(Router);
  if (!localStorage.getItem('madai.accessToken') && !sessionStorage.getItem('madai.accessToken')) {
    return router.createUrlTree(['/login']);
  }

  return isSuperAdminUser() ? true : router.createUrlTree(['/dashboard']);
};
