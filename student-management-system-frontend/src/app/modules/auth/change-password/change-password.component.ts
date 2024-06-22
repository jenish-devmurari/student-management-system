import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { passwordRegex } from 'src/app/constants/constants';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { AuthService } from 'src/app/services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  public changePasswordForm!: FormGroup;
  private subscription: Subscription[] = [] as Subscription[];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.showSecurityAlert()
  }

  private initializeForm(): void {
    this.changePasswordForm = this.fb.group({
      newPassword: [null, [Validators.required, Validators.pattern(passwordRegex)]],
      confirmPassword: [null, [Validators.required, this.confirmPasswordValidator.bind(this)]]
    });
  }

  public confirmPasswordValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const confirmPassword: string = control.value;
    const password: string = this.changePasswordForm?.get('newPassword')?.value;
    this.changePasswordForm?.get('newPassword')?.valueChanges.subscribe((newValue) => {
      if (newValue !== confirmPassword) {
        control.setErrors({ passwordMatch: true });
      } else {
        control.setErrors(null);
      }
    });
    if (confirmPassword !== password) {
      return { passwordMatch: true };
    }
    return null;
  }

  public onSubmit(): void {
    if (this.changePasswordForm.valid) {
      const sub = this.authService.changePassword(this.changePasswordForm.value.confirmPassword).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success) {
            Swal.fire({
              icon: 'success',
              title: 'Password Changed',
              text: 'Your password has been changed successfully. For security reasons, please login again.',
              confirmButtonText: 'OK'
            }).then(() => {
              this.authService.logoutAfterChangePassword();
              this.router.navigate(['auth/login']);
            });
          }
        },
        error: (err) => {
          this.toastr.error(err);
        }
      });
      this.subscription.push(sub);
    } else {
      this.toastr.error('Please fill out the form correctly.', 'Error');
    }
  }

  private showSecurityAlert(): void {
    Swal.fire({
      icon: 'warning',
      title: 'Security Alert',
      text: 'For security reasons, you need to change your password.',
      confirmButtonText: 'OK'
    });
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }
}
