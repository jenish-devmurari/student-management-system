import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IAttendance } from 'src/app/interfaces/attendance.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-student-attendance',
  templateUrl: './student-attendance.component.html',
  styleUrls: ['./student-attendance.component.scss']
})
export class StudentAttendanceComponent {
  public attendances !: IAttendance[];

  constructor(private adminService: AdminService, private route: ActivatedRoute, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.adminService.attendanceData$.subscribe(data => {
      this.attendances = data;
    });
    this.getStudentAttendanceFDetail();
  }

  public getStudentAttendanceFDetail() {
    const id = +this.route.snapshot.parent?.params['id'];
    if (id) {
      this.adminService.getStudentAttendanceDetail(id).subscribe({
        next: (res) => {
          this.attendances = res.data;
        },
        error: (error) => {
          this.toaster.error(error);
        }
      });
    }
  }

  public editAttendance(attendance: IAttendance): void {
    console.log('Editing attendance:', attendance);
  }
}
