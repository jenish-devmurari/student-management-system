import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { TeacherService } from 'src/app/services/teacher.service';
import { ValidationService } from 'src/app/services/validation.service';
import { ToastrService } from 'ngx-toastr';
import { IGradebook } from 'src/app/interfaces/gradebook.interface';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-gradebook-history',
  templateUrl: './gradebook-history.component.html',
  styleUrls: ['./gradebook-history.component.scss']
})
export class GradebookHistoryComponent implements OnInit, OnDestroy {

  public gradesList: IGradebook[] = [] as IGradebook[];
  @ViewChild('closeModal') closeModal!: ElementRef;
  public marksForm!: FormGroup;
  public grade!: IGradebook;

  constructor(
    private teacherService: TeacherService,
    private toaster: ToastrService,
    private fb: FormBuilder, private validation: ValidationService) { }

  ngOnInit(): void {
    this.getAllStudentGrades()
    this.initializeForm();
  }

  private getAllStudentGrades() {
    const sub = this.teacherService.getAllGradesDetails().subscribe({
      next: (res) => {
        if (res.status === HttpStatusCodes.Success) {
          this.gradesList = res.data
        }
        if (res.status === HttpStatusCodes.BadRequest) {
          this.toaster.error(res.message);
        }
      },
      error: (err) => {
        this.toaster.error(err)
      }
    });
  }

  public editGrade(gradebook: IGradebook): void {
    this.marksForm.get('email')?.disable();
    this.grade = gradebook
    this.marksForm.patchValue({
      email: gradebook.email,
      marks: gradebook.marks,
      totalMarks: gradebook.totalMarks
    })
  }

  public calculatePercentage(marks: number, totalMarks: number): number {
    return (marks / totalMarks) * 100;
  }

  public initializeForm() {
    this.marksForm = this.fb.group({
      email: [null, Validators.required],
      marks: [null, [Validators.required, Validators.min(0)]],
      totalMarks: [null, [Validators.required, Validators.min(0)]]
    }, { Validators: this.validation.marksValidator });
  }

  public onSubmit(): void {
    if (this.marksForm.valid) {
      this.grade.marks = this.marksForm.value.marks;
      this.grade.totalMarks = this.marksForm.value.totalMarks;
      this.teacherService.updateGrades(this.grade).subscribe({
        next: (res) => {
          if (res.status == HttpStatusCodes.Success) {
            const updateGrade = this.gradesList.find(g => g.gradeId == this.grade.gradeId);
            if (updateGrade) {
              updateGrade.marks == this.grade.marks;
              updateGrade.totalMarks == this.grade.totalMarks;
              this.toaster.success(res.message);
            }
          }
          else {
            this.toaster.error(res.data)
          }
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
      this.closeModal.nativeElement.click();
    }
  }


  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }


}
