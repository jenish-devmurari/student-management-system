import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IGradebook } from 'src/app/interfaces/gradebook.interface';
import { IStudent } from 'src/app/interfaces/student.interface';
import { TeacherService } from 'src/app/services/teacher.service';

@Component({
  selector: 'app-student-detail',
  templateUrl: './student-detail.component.html',
  styleUrls: ['./student-detail.component.scss']
})
export class StudentDetailComponent {
  public student !: IStudent;
  public studentGrades!: IGradebook[];
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private teacherService: TeacherService, private route: ActivatedRoute, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.getStudentFDetail();
    this.getStudentGradesDetail()
  }

  private getStudentFDetail(): void {
    const id = +this.route.snapshot.params['id']
    if (id) {
      const sub = this.teacherService.getStudentDetailById(id).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success) {
            this.student = res.data;
          } else {
            this.toaster.error(res.message);
          }
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
      this.subscription.push(sub);
    }
  }

  private getStudentGradesDetail(): void {
    const id = +this.route.snapshot.params['id']
    if (id) {
      const sub = this.teacherService.getStudentGradesDetailById(id).subscribe({
        next: (res) => {
          if (res.status === HttpStatusCodes.Success) {
            this.studentGrades = res.data;
          } else {
            this.toaster.error(res.message);
          }
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
      this.subscription.push(sub);
    }
  }

  public calculatePercentage(marks: number, totalMarks: number): number {
    return (marks / totalMarks) * 100;
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }
}
