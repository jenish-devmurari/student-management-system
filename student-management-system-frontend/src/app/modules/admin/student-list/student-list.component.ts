import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { emailRegex } from 'src/app/constants/constants';
import { Classes } from 'src/app/enums/classes.enum';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IStudent } from 'src/app/interfaces/student.interface';
import { AdminService } from 'src/app/services/admin.service';
import { ValidationService } from 'src/app/services/validation.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent implements OnInit, OnDestroy {
  @ViewChild('closeModal') closeModal!: ElementRef;
  public students !: IStudent[];
  private selectedStudent: IStudent | undefined
  public studentEditForm !: FormGroup
  private classId !: number;
  private subscriptions !: Subscription[]

  constructor(private adminService: AdminService, private toaster: ToastrService, private router: Router, private validation: ValidationService) {
  }

  ngOnInit(): void {
    this.adminService.studentData$.subscribe(data => {
      this.students = data;
    });
    this.getAllStudentData();
    this.initializeForm()
  }

  public getAllStudentData(): void {
    // get api
    const getStudentSubscription = this.adminService.getAllStudent().subscribe({
      next: (res) => {
        if (res.status == HttpStatusCodes.Success) {
          this.students = res.data;
        } else {
          this.toaster.error(res.message);
        }
      },
      error: (err) => {
        this.toaster.error(err);
      }
    });
    // this.subscriptions.push(getStudentSubscription);
  }

  public editStudent(student: IStudent): void {
    this.studentEditForm.get('email')?.disable();
    this.selectedStudent = this.students.find(s => s.id === student.id)
    this.classId = student.classId;
    this.studentEditForm.patchValue({
      name: student.name,
      email: student.email,
      rollNumber: student.rollNumber,
      dateOfBirth: student.dateOfBirth.split('T')[0],
      dateOfEnrollment: student.dateOfEnrollment.split('T')[0],
    });
  }

  public deleteStudent(student: IStudent): void {
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
        this.selectedStudent = this.students.find(s => s.id === student.id);
        // delete api
        const deleteStudentSubscription = this.adminService.deleteStudent(this.selectedStudent?.id).subscribe({
          next: (res) => {
            if (res.status === HttpStatusCodes.Success) {
              this.students = this.students.filter(s => s.id !== this.selectedStudent?.id)
              this.toaster.success("Student Deleted Successfully");
            }
            if (res.status === HttpStatusCodes.BadRequest) {
              this.toaster.error(res.message);
            }
          },
          error: (err) => {
            this.toaster.error(err)
          }
        });
        // this.subscriptions.push(deleteStudentSubscription);
      }
    });
  }

  private initializeForm(): void {
    this.studentEditForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.pattern(emailRegex)]),
      rollNumber: new FormControl(null, [Validators.required, Validators.pattern('^[0-9]*$'), this.validation.positiveNumberValidator.bind(this)]),
      dateOfBirth: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator.bind(this)]),
      dateOfEnrollment: new FormControl(null, [Validators.required, this.validation.notFutureDateValidator, this.validation.dateOfBirthBeforeEnrollmentValidator()]),
    });
  }

  public onSubmit(): void {
    if (this.studentEditForm.valid) {
      const formValues = {
        ...this.studentEditForm.value,
        classId: this.classId
      };
      // update api 
      const putStudentSubscription = this.adminService.updateStudent(formValues, this.selectedStudent?.id).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success) {
            const updatedStudentIndex = this.students.findIndex(student => student.id === this.selectedStudent?.id);
            if (updatedStudentIndex !== -1) {
              this.students[updatedStudentIndex] = { ...this.selectedStudent, ...formValues };
            }
            this.toaster.success("Student Update Successfully");
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
      // this.subscriptions.push(putStudentSubscription);
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
    this.studentEditForm.reset();
  }

  get classes(): string[] {
    return Object.keys(Classes).filter(key => isNaN(Number(key)));
  }

  ngOnDestroy(): void {
    // this.subscriptions.forEach(sub => sub.unsubscribe());
  }

}
