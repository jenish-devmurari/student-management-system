import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { emailRegex } from 'src/app/constants/constants';
import { Classes } from 'src/app/enums/classes.enum';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Roles } from 'src/app/enums/roles.enum';
import { ITeacher } from 'src/app/interfaces/teacher.interface';
import { AdminService } from 'src/app/services/admin.service';
import { AuthService } from 'src/app/services/auth.service';
import { TeacherService } from 'src/app/services/teacher.service';
import { ValidationService } from 'src/app/services/validation.service';

@Component({
  selector: 'app-register-student',
  templateUrl: './register-student.component.html',
  styleUrls: ['./register-student.component.scss']
})
export class RegisterStudentComponent implements OnInit {
  public studentRegisterForm !: FormGroup;
  public role: string | null = "";
  public userDetails !: any;
  public classId: number = 0;
  public className !: string;

  constructor(private validation: ValidationService, private adminService: AdminService, private toaster: ToastrService, private authService: AuthService, private teacherService: TeacherService) {

  }


  ngOnInit(): void {
    this.initializeForm();
    this.role = this.authService.getUserRole();
    this.authService.loggedInUserDetails().subscribe(
      (res) => {
        this.userDetails = res.data;
        this.classId = this.userDetails.classId;
        this.className = Classes[this.classId];
        this.customizeForm();
      },
      (error) => {
        this.toaster.error('Error fetching user details', error);
      }
    );
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
      if (this.role == Roles.Admin) {
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
      }

      if (this.role == Roles.Teacher) {
        formValues.class = this.classId;
        this.teacherService.addStudent(this.studentRegisterForm.value).subscribe({
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
        this.customizeForm();
      }
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

  private customizeForm() {
    if (this.role === Roles.Teacher) {
      this.studentRegisterForm.patchValue({
        class: this.className
      });
      if (this.role == Roles.Teacher) {
        this.studentRegisterForm.get('class')?.disable();
      }
    }
  }

}
