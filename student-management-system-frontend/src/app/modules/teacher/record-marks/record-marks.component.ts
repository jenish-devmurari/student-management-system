import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription, debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Subjects } from 'src/app/enums/subjects.enum';
import { IResponse } from 'src/app/interfaces/response.interface';
import { IStudent } from 'src/app/interfaces/student.interface';
import { IUser } from 'src/app/interfaces/user.interface';
import { AuthService } from 'src/app/services/auth.service';
import { TeacherService } from 'src/app/services/teacher.service';
import { ValidationService } from 'src/app/services/validation.service';

@Component({
  selector: 'app-record-marks',
  templateUrl: './record-marks.component.html',
  styleUrls: ['./record-marks.component.scss']
})
export class RecordMarksComponent implements OnInit, OnDestroy {
  public marksForm!: FormGroup;
  public students: IStudent[] = [] as IStudent[];
  public userDetails: IUser = {} as IUser
  public subjectId: number | undefined = 0;
  public subjectName !: string;
  public emailSuggestions: string[] = [];
  public noResultsFound: boolean = false;
  private subscription: Subscription[] = [] as Subscription[];

  constructor(
    private teacherService: TeacherService,
    private validation: ValidationService,
    private authService: AuthService,
    private toaster: ToastrService
  ) { }

  ngOnInit() {
    this.initializeForm();
    const sub = this.authService.loggedInUserDetails().subscribe(
      (res) => {
        this.userDetails = res.data;
        this.subjectId = this.userDetails.subjectId;
        if (this.subjectId) {
          this.subjectName = Subjects[this.subjectId];
        }
      }
    );
    const searchEmailSubscription = this.marksForm
      .get('email')
      ?.valueChanges.pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((value) => this.searchEmail(value))
      )
      .subscribe((emails: string[]) => {
        this.emailSuggestions = emails;
        this.noResultsFound = emails.length === 0;
      });
    this.getAllStudentOfTeacherClass();
  }

  private initializeForm(): void {
    this.marksForm = new FormGroup({
      email: new FormControl(null, [Validators.required, this.emailFromListValidator()]),
      date: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator]),
      marks: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]),
      totalMarks: new FormControl(null, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)])
    }, { validators: this.validation.marksTotalMarksValidator });
  }

  private getAllStudentOfTeacherClass(): void {
    const sub = this.teacherService.getAllStudentOfTeacherClass().subscribe({
      next: (res) => {
        if (res.status === HttpStatusCodes.Success) {
          this.students = res.data;
        }
      },
      error: (err) => {
        this.toaster.error('Error fetching student list:', err);
      }
    });
    this.subscription.push(sub);
  }

  private searchEmail(query: string): Observable<string[]> {
    if (query.length < 3) {
      return of([]);
    }
    return this.teacherService.getStudentEmailListBasedOnSearch(query).pipe(
      switchMap((response: IResponse) => {
        return of(response.data);
      })
    );
  }

  private emailFromListValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (this.emailSuggestions.includes(control.value)) {
        return null;
      }
      return { emailNotFromList: true };
    };
  }

  public onSubmit(): void {
    if (this.marksForm.valid) {
      const sub = this.teacherService.marksAdd(this.marksForm.value).subscribe({
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
      this.subscription.push(sub);
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

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }

}
