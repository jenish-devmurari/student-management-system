import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription, take } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IGradebook } from 'src/app/interfaces/gradebook.interface';
import { StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-subject-marks',
  templateUrl: './subject-marks.component.html',
  styleUrls: ['./subject-marks.component.scss']
})
export class SubjectMarksComponent implements OnInit, OnDestroy {
  public subjectId: number = 0 as number;
  public gradebooks: IGradebook[] = [] as IGradebook[];
  private subscriptions: Subscription[] = [] as Subscription[];

  constructor(private route: ActivatedRoute, private studentService: StudentService, private toaster: ToastrService) { }


  ngOnInit(): void {
    const sub = this.route.params.subscribe(params => {
      this.subjectId = +params['subjectId'];
      this.getSubjectMarks();
    });
    this.subscriptions.push(sub);
  }

  private getSubjectMarks() {
    const sub = this.studentService.getStudentGradesBasedOnSubject(this.subjectId).subscribe({
      next: (res) => {
        this.gradebooks = res.data
      },
      error: (err) => {
        this.toaster.error(err);
      }
    });
    this.subscriptions.push(sub);
  }

  public calculatePercentage(marks: number, totalMarks: number): number {
    return (marks / totalMarks) * 100;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
