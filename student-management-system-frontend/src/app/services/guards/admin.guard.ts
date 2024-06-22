import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';
import { Roles } from 'src/app/enums/roles.enum';
import Swal from 'sweetalert2';

export const adminGuard: CanActivateFn = (route, state) => {

  const authService: AuthService = inject(AuthService);
  const toaster: ToastrService = inject(ToastrService);
  const router: Router = inject(Router);

  if (authService.isLoggedIn()) {
    const userRole: string | null = authService.getUserRole();
    if (userRole === Roles.Admin) {
      return true;
    } else {
      toaster.error("Unauthorize access.");
      if (userRole === Roles.Teacher) {
        router.navigate(['teacher']);
        return false;
      } else {
        router.navigate(['student']);
        return false;
      }
    }
  } else {
    Swal.fire({
      icon: 'error',
      title: 'Not Logged In',
      text: 'You are not logged in. Please log in first.',
      confirmButtonText: 'OK'
    }).then(() => {
      router.navigate(['auth/login']);
    });
    return false;
  }
};
