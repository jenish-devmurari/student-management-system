import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Observable, map, startWith } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Subjects } from 'src/app/enums/subjects.enum';
import { IStudent } from 'src/app/interfaces/student.interface';
import { ITeacher } from 'src/app/interfaces/teacher.interface';
import { AuthService } from 'src/app/services/auth.service';
import { TeacherService } from 'src/app/services/teacher.service';
import { ValidationService } from 'src/app/services/validation.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-record-marks',
  templateUrl: './record-marks.component.html',
  styleUrls: ['./record-marks.component.scss']
})
export class RecordMarksComponent {
  public marksForm!: FormGroup;
  public students: IStudent[] = [] as IStudent[];
  public userDetails !: any
  public subjectId: number = 0;
  public subjectName !: string;

  constructor(
    private teacherService: TeacherService,
    private validation: ValidationService,
    private authService: AuthService,
    private toaster: ToastrService
  ) { }

  ngOnInit() {
    this.initializeForm();
    this.authService.loggedInUserDetails().subscribe(
      (res) => {
        this.userDetails = res.data;
        this.subjectId = this.userDetails.subjectId;
        this.subjectName = Subjects[this.subjectId];
      }
    )
    this.getAllStudentOfTeacherClass();
  }

  private initializeForm() {
    this.marksForm = new FormGroup({
      email: new FormControl(null, Validators.required),
      date: new FormControl(null, [Validators.required, this.validation.notFutureDate]),
      marks: new FormControl(null, [Validators.required, Validators.min(0)]),
      totalMarks: new FormControl(null, [Validators.required, Validators.min(0)])
    }, { validators: this.validation.marksValidator });
  }

  private getAllStudentOfTeacherClass() {
    this.teacherService.getAllStudentOfTeacherClass().subscribe({
      next: (res) => {
        if (res.status === HttpStatusCodes.Success) {
          this.students = res.data;
        }
      },
      error: (err) => {
        this.toaster.error('Error fetching student list:', err);
      }
    });
  }

  public onSubmit() {
    if (this.marksForm.valid) {
      this.teacherService.marksAdd(this.marksForm.value).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success) {
            this.toaster.success(res.message);
          }
          if (res.status === HttpStatusCodes.BadRequest) {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err)
        }
      });
    } else {
      alert("Please fill up form");
    }
    this.marksForm.reset();
  }

  public validationClass(control: AbstractControl | null): { [key: string]: boolean | undefined } {
    return this.validation.validationNgClass(control);
  }

  public isFieldInvalid(control: AbstractControl | null): boolean {
    return this.validation.isFormFieldInvalid(control);
  }

}
