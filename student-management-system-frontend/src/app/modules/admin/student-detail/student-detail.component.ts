import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { IStudent } from 'src/app/interfaces/student.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-student-detail',
  templateUrl: './student-detail.component.html',
  styleUrls: ['./student-detail.component.scss']
})
export class StudentDetailComponent implements OnInit, OnDestroy {
  public student !: IStudent;
  public subscription: Subscription[] = [] as Subscription[]

  constructor(private adminService: AdminService, private route: ActivatedRoute, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.getStudentFDetail();
  }

  public getStudentFDetail(): void {
    const id = +this.route.snapshot.params['id']
    if (id) {
      const sub = this.adminService.getStudentDetailById(id).subscribe({
        next: (res) => {
          this.student = res.data;
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
      this.subscription.push(sub);
    }
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }
}
