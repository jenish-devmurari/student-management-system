import { HttpStatusCode } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendance } from 'src/app/interfaces/attendance.interface';
import { TeacherService } from 'src/app/services/teacher.service';

@Component({
  selector: 'app-attendance-history',
  templateUrl: './attendance-history.component.html',
  styleUrls: ['./attendance-history.component.scss']
})
export class AttendanceHistoryComponent implements OnInit, OnDestroy {
  public attendanceList: IAttendance[] = [] as IAttendance[];
  private currentDate: string = new Date().toISOString().split('T')[0];
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private teacherService: TeacherService, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.getAttendanceHistory();
  }

  private getAttendanceHistory(): void {
    const sub = this.teacherService.getAttendanceHistory().subscribe(
      {
        next: (res) => {
          if (res.status == HttpStatusCodes.Success) {
            this.attendanceList = res.data;
          } else {
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

  public onEdit(attendance: IAttendance): void {
    if (this.canEdit(attendance)) {
      attendance.isPresent = !attendance.isPresent;
      const updateAttendance: IAttendance | undefined = this.attendanceList.find(a => a.id == attendance.id);
      if (updateAttendance) {
        const sub = this.teacherService.updateAttendance(attendance).subscribe({
          next: (res) => {
            if (res.status == HttpStatusCodes.Success) {
              updateAttendance.isPresent == attendance.isPresent;
              this.toaster.success(res.message);
            }
          },
          error: (err) => {
            this.toaster.error(err);
          }
        });
        this.subscription.push(sub);
      }
    } else {
      this.toaster.error('Editing is allowed only for today\'s attendance.');
    }
  }

  public canEdit(attendance: IAttendance): boolean {
    const date: string = attendance.date.split('T')[0];
    return date === this.currentDate;
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }
}
