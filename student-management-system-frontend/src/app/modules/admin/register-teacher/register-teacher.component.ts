import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { emailRegex } from 'src/app/constants/constants';
import { Classes } from 'src/app/enums/classes.enum';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Qualification } from 'src/app/enums/qualifications.enum';
import { Subjects } from 'src/app/enums/subjects.enum';
import { AdminService } from 'src/app/services/admin.service';
import { ValidationService } from 'src/app/services/validation.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-register-teacher',
  templateUrl: './register-teacher.component.html',
  styleUrls: ['./register-teacher.component.scss']
})
export class RegisterTeacherComponent implements OnInit, OnDestroy {
  public teacherRegisterForm !: FormGroup;
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private validation: ValidationService, private toaster: ToastrService, private adminService: AdminService) {
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  public onSubmit(): void {
    if (this.teacherRegisterForm.valid) {
      const formValues = this.teacherRegisterForm.value;
      formValues.subject = Subjects[formValues.subject as keyof typeof Subjects];
      formValues.class = Classes[formValues.class as keyof typeof Classes];
      const sub = this.adminService.addTeacher(this.teacherRegisterForm.value).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Created) {
            this.toaster.success("Teacher Register Successfully");
          }
          if (res.status === HttpStatusCodes.BadRequest) {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err)
        }
      });
      this.subscription.push(sub);
      this.resetForm();
    } else {
      Swal.fire("Please fill up all required field in form")
    }
  }

  public validationClass(control: AbstractControl | null): { [key: string]: boolean | undefined } {
    return this.validation.validationNgClass(control);
  }

  public isFieldInvalid(control: AbstractControl | null): boolean {
    return this.validation.isFormFieldInvalid(control);
  }

  public emailPattern(control: AbstractControl | null): boolean {
    return this.validation.isEmailPatternMatch(control);
  }

  public resetForm(): void {
    this.teacherRegisterForm.reset();
  }

  get qualifications(): string[] {
    return Object.values(Qualification);
  }
  get subjects(): string[] {
    return Object.keys(Subjects).filter(key => isNaN(Number(key)));
  }

  get classes(): string[] {
    return Object.keys(Classes).filter(key => isNaN(Number(key)));
  }

  private initializeForm(): void {
    this.teacherRegisterForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.pattern(emailRegex)]),
      class: new FormControl(null, [Validators.required]),
      dateOfBirth: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator]),
      dateOfEnrollment: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator, this.validation.dateOfBirthBeforeEnrollmentValidator()]),
      subject: new FormControl(null, [Validators.required]),
      qualification: new FormControl(null, [Validators.required]),
      salary: new FormControl(null, [Validators.required, this.validation.positiveNumberValidator])
    }, { validators: this.validation.dateOfBirthBeforeEnrollmentValidator() });
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }
}
