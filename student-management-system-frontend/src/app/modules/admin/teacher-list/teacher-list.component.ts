import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { emailRegex } from 'src/app/constants/constants';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Qualification } from 'src/app/enums/qualifications.enum';
import { ITeacher } from 'src/app/interfaces/teacher.interface';
import { AdminService } from 'src/app/services/admin.service';
import { ValidationService } from 'src/app/services/validation.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-teacher-list',
  templateUrl: './teacher-list.component.html',
  styleUrls: ['./teacher-list.component.scss']
})
export class TeacherListComponent implements OnInit, OnDestroy {
  @ViewChild('closeModal') closeModal!: ElementRef;
  public teachers: ITeacher[] = [] as ITeacher[];
  public selectedTeacher: ITeacher | undefined
  public teacherEditForm !: FormGroup
  public currentPage = 1;
  public itemsPerPage = 10;
  public totalItems = 0;
  private subscriptions: Subscription[] = [] as Subscription[];

  constructor(private adminService: AdminService, private toaster: ToastrService, private router: Router, private validation: ValidationService) {
  }

  ngOnInit(): void {
    const sub = this.adminService.teacherData$.subscribe(data => {
      this.teachers = data;
      this.totalItems = data.length;
    });
    this.subscriptions.push(sub);
    this.getAllTeacherData();
    this.initializeForm();
  }

  public editTeacher(teacher: ITeacher): void {
    this.teacherEditForm.get('email')?.disable();
    this.selectedTeacher = teacher;
    this.teacherEditForm.patchValue({
      name: teacher.name,
      email: teacher.email,
      qualification: teacher.qualification,
      dateOfBirth: teacher.dateOfBirth.split('T')[0],
      dateOfEnrollment: teacher.dateOfEnrollment.split('T')[0],
      salary: teacher.salary
    });
  }

  public deleteTeacher(teacher: ITeacher): void {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        // delete api
        const deleteTeacherSubscription = this.adminService.deleteTeacher(teacher.id).subscribe({
          next: (res) => {
            if (res.status === HttpStatusCodes.Success) {
              this.teachers = this.teachers.filter(s => s.id !== teacher.id)
              this.toaster.success("Teacher Deleted Successfully");
            }
            if (res.status === HttpStatusCodes.BadRequest) {
              this.toaster.error(res.message);
            }
          },
          error: (err) => {
            this.toaster.error(err)
          }
        });
        this.subscriptions.push(deleteTeacherSubscription);
      }
    });
  }

  public onSubmit(): void {
    if (this.teacherEditForm.valid) {
      const formValues = {
        ...this.teacherEditForm.value,
      };
      // update api 
      const putTeacherSubscription = this.adminService.updateTeacher(formValues, this.selectedTeacher?.id).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success) {
            const updatedTeacherIndex = this.teachers.findIndex(teacher => teacher.id === this.selectedTeacher?.id);
            if (updatedTeacherIndex !== -1) {
              this.teachers[updatedTeacherIndex] = { ...this.selectedTeacher, ...formValues };
            }
            this.toaster.success("Teacher Update Successfully");
          }
          if (res.status === HttpStatusCodes.BadRequest) {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err)
        }
      });
      this.closeModal.nativeElement.click();
      this.subscriptions.push(putTeacherSubscription);
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
    this.teacherEditForm.reset();
  }

  get qualifications(): string[] {
    return Object.values(Qualification);
  }

  public onPageChange(page: number): void {
    this.currentPage = page;
  }

  private getAllTeacherData(): void {
    // get api
    const getTeacherSubscription = this.adminService.getAllTeacher().subscribe({
      next: (res) => {
        if (res.status == HttpStatusCodes.Success) {
          this.teachers = res.data;
          this.totalItems = res.data.length;
        } else {
          this.toaster.error(res.message);
        }
      },
      error: (err) => {
        this.toaster.error(err);
      }
    });
    this.subscriptions.push(getTeacherSubscription);
  }

  private initializeForm(): void {
    this.teacherEditForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.pattern(emailRegex)]),
      dateOfBirth: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator.bind(this)]),
      qualification: new FormControl(null, [Validators.required]),
      dateOfEnrollment: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator, this.validation.dateOfBirthBeforeEnrollmentValidator()]),
      salary: new FormControl(null, [Validators.required, this.validation.positiveNumberValidator])
    }, { validators: this.validation.dateOfBirthBeforeEnrollmentValidator() });
  }

  ngOnDestroy(): void {
    if (this.subscriptions.length > 0) {
      this.subscriptions.forEach(sub => sub.unsubscribe());
    }
  }
}
