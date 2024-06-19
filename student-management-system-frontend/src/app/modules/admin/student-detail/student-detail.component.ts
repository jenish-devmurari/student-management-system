import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IStudent } from 'src/app/interfaces/student.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-student-detail',
  templateUrl: './student-detail.component.html',
  styleUrls: ['./student-detail.component.scss']
})
export class StudentDetailComponent {
  public student !: IStudent;

  constructor(private adminService: AdminService, private route: ActivatedRoute, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.getStudentFDetail();
  }

  public getStudentFDetail() {
    const id = +this.route.snapshot.params['id']
    if (id) {
      this.adminService.getStudentDetailById(id).subscribe({
        next: (res) => {
          this.student = res.data;
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
    }
  }
}
