import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IGradebook } from 'src/app/interfaces/gradebook.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-student-gradebook',
  templateUrl: './student-gradebook.component.html',
  styleUrls: ['./student-gradebook.component.scss']
})
export class StudentGradebookComponent {
  public gradebooks !: IGradebook[];

  constructor(private adminService: AdminService, private route: ActivatedRoute, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.adminService.gradebookData$.subscribe(data => {
      this.gradebooks = data;
    });
    this.getStudentGradeBookDetail();
  }

  public getStudentGradeBookDetail() {
    const id = +this.route.snapshot.parent?.params['id'];
    if (id) {
      this.adminService.getStudentGradeBookDetail(id).subscribe({
        next: (res) => {
          this.gradebooks = res.data;
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
    }
  }

  public editGrade(gradebook: IGradebook): void {
    console.log('Editing gradebook:', gradebook);
  }

  public calculatePercentage(marks: number, totalMarks: number): number {
    return (marks / totalMarks) * 100;
  }
}
