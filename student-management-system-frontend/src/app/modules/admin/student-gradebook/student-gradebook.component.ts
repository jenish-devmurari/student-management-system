import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IGradebook } from 'src/app/interfaces/gradebook.interface';
import { AdminService } from 'src/app/services/admin.service';
import { ValidationService } from 'src/app/services/validation.service';

@Component({
  selector: 'app-student-gradebook',
  templateUrl: './student-gradebook.component.html',
  styleUrls: ['./student-gradebook.component.scss']
})
export class StudentGradebookComponent implements OnInit {
  @ViewChild('closeModal') closeModal!: ElementRef;
  public gradebooks!: IGradebook[];
  public marksForm!: FormGroup;
  public grade!: IGradebook;

  constructor(private adminService: AdminService, private route: ActivatedRoute, private toaster: ToastrService, private fb: FormBuilder, private validation: ValidationService) {
  }

  ngOnInit(): void {
    this.adminService.gradebookData$.subscribe(data => {
      this.gradebooks = data;
    });
    this.initializeForm()
    this.getStudentGradeBookDetail();
  }

  public getStudentGradeBookDetail() {
    const id = +this.route.snapshot.parent?.params['id'];
    if (id) {
      this.adminService.getStudentGradeBookDetail(id).subscribe({
        next: (res) => {
          if (res.status == HttpStatusCodes.Success) {
            this.gradebooks = res.data;
          } else {
            this.toaster.error(res.message)
          }
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
    }
  }

  public editGrade(gradebook: IGradebook): void {
    this.marksForm.get('studentName')?.disable();
    this.marksForm.get('subjectName')?.disable();
    this.grade = gradebook
    this.marksForm.patchValue({
      studentName: gradebook.name,
      subjectName: gradebook.subjectName,
      marks: gradebook.marks,
      totalMarks: gradebook.totalMarks
    })
  }

  public calculatePercentage(marks: number, totalMarks: number): number {
    return (marks / totalMarks) * 100;
  }

  public initializeForm() {
    this.marksForm = this.fb.group({
      studentName: [null, Validators.required],
      subjectName: [null, Validators.required],
      marks: [null, [Validators.required, Validators.min(0)]],
      totalMarks: [null, [Validators.required, Validators.min(0)]]
    }, { Validators: this.validation.marksValidator });
  }

  public onSubmit(): void {
    if (this.marksForm.valid) {
      this.grade.marks = this.marksForm.value.marks;
      this.grade.totalMarks = this.marksForm.value.totalMarks;
      this.adminService.updateGrades(this.grade).subscribe({
        next: (res) => {
          if (res.status == 200) {
            const updateGrade = this.gradebooks.find(g => g.gradeId == this.grade.gradeId);
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


  public validationClass(control: AbstractControl | null): { [key: string]: boolean | undefined } {
    return this.validation.validationNgClass(control);
  }

  public isFieldInvalid(control: AbstractControl | null): boolean {
    return this.validation.isFormFieldInvalid(control);
  }

  public emailPattern(control: AbstractControl | null): boolean {
    return this.validation.isEmailPatternMatch(control);
  }
}
