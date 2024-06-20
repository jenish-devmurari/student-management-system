import { HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendance } from 'src/app/interfaces/attendance.interface';
import { TeacherService } from 'src/app/services/teacher.service';

@Component({
  selector: 'app-attendance-history',
  templateUrl: './attendance-history.component.html',
  styleUrls: ['./attendance-history.component.scss']
})
export class AttendanceHistoryComponent implements OnInit {
  public attendanceList: IAttendance[] = [] as IAttendance[];
  private currentDate: string = new Date().toISOString().split('T')[0];

  constructor(private teacherService: TeacherService, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.getAttendanceHistory();
  }

  private getAttendanceHistory(): void {
    this.teacherService.getAttendanceHistory().subscribe(
      {
        next: (res) => {
          if (res.status == HttpStatusCodes.Success) {
            this.attendanceList = this.sortByDateDescending(res.data);
          } else {
            this.toaster.error(res.message);
          }
        },
        error: (err) => {
          this.toaster.error(err);
        }
      }
    )
  }

  public onEdit(attendance: IAttendance): void {
    if (this.canEdit(attendance)) {
      attendance.isPresent = !attendance.isPresent;
      const updateAttendance: IAttendance | undefined = this.attendanceList.find(a => a.id == attendance.id);
      if (updateAttendance) {
        this.teacherService.updateAttendance(attendance).subscribe({
          next: (res) => {
            if (res.status == HttpStatusCodes.Success) {
              updateAttendance.isPresent == attendance.isPresent;
              this.toaster.success(res.message);
            }
          },
          error: (err) => {
            this.toaster.error(err);
          }
        })
      }
    } else {
      this.toaster.error('Editing is allowed only for today\'s attendance.');
    }
  }

  public canEdit(attendance: IAttendance): boolean {
    const date: string = attendance.date.split('T')[0];
    return date === this.currentDate;
  }

  private sortByDateDescending(data: IAttendance[]): IAttendance[] {
    return data.sort((a, b) => {
      const dateA = new Date(a.date).getTime();
      const dateB = new Date(b.date).getTime();
      return dateB - dateA;
    });
  }

}
