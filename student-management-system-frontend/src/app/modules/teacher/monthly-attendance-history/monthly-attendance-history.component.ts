import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import flatpickr from 'flatpickr';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendance } from 'src/app/interfaces/attendance.interface';
import { TeacherService } from 'src/app/services/teacher.service';


@Component({
  selector: 'app-monthly-attendance-history',
  templateUrl: './monthly-attendance-history.component.html',
  styleUrls: ['./monthly-attendance-history.component.scss']
})
export class MonthlyAttendanceHistoryComponent {
  public selectedDate: Date | null = null;
  public isDateSelected = false;
  public attendanceDetails: IAttendance[] = [] as IAttendance[];
  public formattedSelectedDate: string | null = null;

  constructor(private http: HttpClient, private teacherService: TeacherService, private toaster: ToastrService) { }

  ngAfterViewInit() {
    flatpickr('#datepicker', {
      dateFormat: 'Y-m-d',
      onChange: this.onDateChange.bind(this)
    });
  }

  public onDateChange(selectedDates: Date[]): void {
    if (selectedDates.length > 0) {
      this.selectedDate = selectedDates[0];
      this.selectedDate.setUTCDate(this.selectedDate.getUTCDate() + 1);
      this.fetchAttendanceDetails(this.selectedDate);
    }
  }

  private fetchAttendanceDetails(date: Date): void {
    this.formattedSelectedDate = date.toISOString().split('T')[0];
    this.teacherService.getAttendanceBasedOnDate(this.formattedSelectedDate).pipe(take(1)).subscribe({
      next: (res) => {
        if (res.status == HttpStatusCodes.Success) {
          this.attendanceDetails = res.data;
          this.isDateSelected = true
        }
      },
      error: (err) => { this.toaster.error(err) }
    }
    );
  }
}
