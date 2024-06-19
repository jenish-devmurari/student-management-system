import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { emailRegex } from 'src/app/constants/constants';
import { Classes } from 'src/app/enums/classes.enum';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { AdminService } from 'src/app/services/admin.service';
import { ValidationService } from 'src/app/services/validation.service';

@Component({
  selector: 'app-register-student',
  templateUrl: './register-student.component.html',
  styleUrls: ['./register-student.component.scss']
})
export class RegisterStudentComponent implements OnInit {
  public studentRegisterForm !: FormGroup;
  constructor(private validation: ValidationService, private adminService: AdminService, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.studentRegisterForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.pattern(emailRegex)]),
      class: new FormControl(null, [Validators.required]),
      rollNumber: new FormControl(null, [Validators.required, Validators.pattern('^[0-9]*$'), this.validation.positiveNumberValidator.bind(this)]),
      dateOfBirth: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator.bind(this)]),
      dateOfEnrollment: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator, this.validation.dateOfBirthBeforeEnrollmentValidator()]),
    });
  }

  public onSubmit(): void {
    if (this.studentRegisterForm.valid) {
      const formValues = this.studentRegisterForm.value;
      formValues.class = Classes[formValues.class as keyof typeof Classes];
      console.log('Form Submitted', this.studentRegisterForm.value);
      this.adminService.addStudent(this.studentRegisterForm.value).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Created) {
            this.toaster.success("Student Register Successfully");
          }
          if (res.status === HttpStatusCodes.BadRequest) {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err)
        }
      });
      this.resetForm();
    } else {
      alert("Please fill form field");
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
    this.studentRegisterForm.reset();
  }

  get classes(): string[] {
    return Object.keys(Classes).filter(key => isNaN(Number(key)));
  }


}
