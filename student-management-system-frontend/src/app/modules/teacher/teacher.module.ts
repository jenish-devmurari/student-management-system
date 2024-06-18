import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeacherRoutingModule } from './teacher-routing.module';
import { TeacherComponent } from './teacher.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AttendanceComponent } from './attendance/attendance.component';
import { GradebookComponent } from './gradebook/gradebook.component';
import { RecordAttendanceComponent } from './record-attendance/record-attendance.component';
import { AttendanceHistoryComponent } from './attendance-history/attendance-history.component';
import { MonthlyAttendanceHistoryComponent } from './monthly-attendance-history/monthly-attendance-history.component';
import { RecordMarksComponent } from './record-marks/record-marks.component';
import { GradebookHistoryComponent } from './gradebook-history/gradebook-history.component';
import { StudentDetailComponent } from './student-detail/student-detail.component';


@NgModule({
  declarations: [
    TeacherComponent,
    AttendanceComponent,
    GradebookComponent,
    RecordAttendanceComponent,
    AttendanceHistoryComponent,
    MonthlyAttendanceHistoryComponent,
    RecordMarksComponent,
    GradebookHistoryComponent,
    StudentDetailComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    SharedModule,
    TeacherRoutingModule
  ]
})
export class TeacherModule { }
