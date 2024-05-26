import { CanActivateFn } from '@angular/router';
import { inject } from "@angular/core";
import { UserService } from "./user.service";
import { Router } from '@angular/router';

export const authGuard: CanActivateFn = async (route, state) => {
  const userService = inject(UserService); // Use Angular's inject function to get the service instance
  const router = inject(Router); // Similarly, inject the Router

  console.log("suicide")

  try {
    const user = await userService.getCurrent();
    console.log(user);
    if (user) {
      console.log("must do")
      return true;
    } else {
      router.navigate(['/login']);
      console.log("nah not today")
      return false;
    }
  } catch (error) {
    console.error(error);
    return false; // Or handle the error appropriately
  }
};
