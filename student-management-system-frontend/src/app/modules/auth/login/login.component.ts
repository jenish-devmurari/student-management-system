import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Roles } from 'src/app/enums/roles.enum';
import { ILogin } from 'src/app/interfaces/login.interface';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  @ViewChild('loginForm') loginForm!: NgForm;

  constructor(private toaster: ToastrService, private authService: AuthService, private router: Router) {
  }

  public loginData: ILogin = {
    email: '',
    password: ''
  }

  public onSubmit(): void {
    if (this.loginForm.valid) {
      this.authService.login(this.loginData).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success && res.data?.isPasswordReset === true) {
            this.toaster.success('Login Successful');
            const userRole: string | null = this.authService.getUserRole()
            this.routeBasedOnRole(userRole);
          }
          if (res.status === HttpStatusCodes.Unauthorized) {
            this.toaster.error(res.message);
          }
          if (res.status === HttpStatusCodes.NotFound) {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err)
        }
      });
      this.loginForm.reset();
    } else {
      this.toaster.error("Please fill out the login detail.", 'Validation Error');
    }
  }

  private routeBasedOnRole(role: string | null): void {
    switch (role) {
      case Roles.Admin:
        this.router.navigate(['admin']);
        break;
      case Roles.Teacher:
        this.router.navigate(['teacher']);
        break;
      case Roles.Student:
        this.router.navigate(['student']);
        break;
      default:
        this.router.navigate(['auth/login']);
        break;
    }
  }
}
