import { CanActivateFn } from '@angular/router';
import { AuthService } from './auth.service';
import { NotificationService } from './notification.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const notification = inject(NotificationService);

  if (auth.isloggedIn()) {
    return true;
  } else {
    notification.showMessage('You must be logged in to access this page');
    auth.logout();
    return false;
  }
};

