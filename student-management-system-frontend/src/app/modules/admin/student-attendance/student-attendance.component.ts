import { HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendance } from 'src/app/interfaces/attendance.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-student-attendance',
  templateUrl: './student-attendance.component.html',
  styleUrls: ['./student-attendance.component.scss']
})
export class StudentAttendanceComponent {
  public attendances !: IAttendance[];
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private adminService: AdminService, private route: ActivatedRoute, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    const sub = this.adminService.attendanceData$.subscribe(data => {
      this.attendances = data;
    });
    this.subscription.push(sub);
    this.getStudentAttendanceFDetail();
  }

  public getStudentAttendanceFDetail() {
    const id = +this.route.snapshot.parent?.params['id'];
    if (id) {
      const sub = this.adminService.getStudentAttendanceDetail(id).subscribe({
        next: (res) => {
          this.attendances = res.data;
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
      this.subscription.push(sub);
    }
  }

  public editAttendance(attendance: IAttendance): void {
    attendance.isPresent = !attendance.isPresent
    const sub = this.adminService.updateAttendance(attendance).subscribe({
      next: (res) => {
        if (res.status == HttpStatusCodes.Success) {
          const updatedAttendance = this.attendances.find(a => a.id == attendance.id);
          if (updatedAttendance) {
            updatedAttendance.isPresent = attendance.isPresent;
            this.toaster.success(res.message);
          }
        } else {
          this.toaster.error(res.message)
        }
      },
      error: (error) => {
        this.toaster.error(error);
      }
    })
    this.subscription.push(sub);
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }
}
