import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { Roles } from 'src/app/enums/roles.enum';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {

  const authService: AuthService = inject(AuthService);
  const toaster: ToastrService = inject(ToastrService);
  const router: Router = inject(Router);

  if (authService.isLoggedIn()) {
    const userRole: string | null = authService.getUserRole();
    switch (userRole) {
      case Roles.Admin:
        toaster.error("You can not go back you need to logout");
        router.navigate(['admin']);
        return false;
      case Roles.Teacher:
        toaster.error("Teachers cannot access this route. Please log out first.");
        router.navigate(['teacher']);
        return false;
      case Roles.Student:
        toaster.error("Students cannot access this route. Please log out first.");
        router.navigate(['student']);
        return false;
      default:
        return true;
    }
  }

  return true;
};
