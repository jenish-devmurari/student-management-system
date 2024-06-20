import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendance } from 'src/app/interfaces/attendance.interface';
import { StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.scss']
})
export class AttendanceComponent implements OnInit, OnDestroy {
  public attendanceList: IAttendance[] = [] as IAttendance[];
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private studentService: StudentService, private toaster: ToastrService) {
  }
  ngOnInit(): void {
    this.getAttendanceOfStudent();
  }

  private getAttendanceOfStudent() {
    const sub = this.studentService.getAttendanceOfStudent().subscribe(
      {
        next: (res) => {
          if (res.status == HttpStatusCodes.Success) {
            this.attendanceList = res.data
          }
          else {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err);
        }
      }
    );
    this.subscription.push(sub);
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe())
    }
  }
}


