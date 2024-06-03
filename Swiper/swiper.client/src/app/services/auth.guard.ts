import { CanActivateFn } from '@angular/router';
import { inject } from "@angular/core";
import { UserService } from "./user.service";
import { Router } from '@angular/router';

export const authGuard: CanActivateFn = async (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);

  try {
    const user = await userService.getCurrent();
    if (user) {
      return true;
    } else {
      router.navigate(['/login']);
      return false;
    }
  } catch (error) {
    return false;
  }
};
