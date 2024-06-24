import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from '../shared/dashboard/dashboard.component';
import { RegisterStudentComponent } from '../shared/register-student/register-student.component';
import { AttendanceHistoryComponent } from './attendance-history/attendance-history.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { GradebookHistoryComponent } from './gradebook-history/gradebook-history.component';
import { GradebookComponent } from './gradebook/gradebook.component';
import { MonthlyAttendanceHistoryComponent } from './monthly-attendance-history/monthly-attendance-history.component';
import { RecordAttendanceComponent } from './record-attendance/record-attendance.component';
import { RecordMarksComponent } from './record-marks/record-marks.component';
import { StudentDetailComponent } from './student-detail/student-detail.component';
import { TeacherComponent } from './teacher.component';

const routes: Routes = [
  {
    path: '', component: TeacherComponent, pathMatch: 'prefix',
    children: [
      {
        path: '', redirectTo: 'dashboard', pathMatch: 'full',
      },
      {
        path: 'dashboard', component: DashboardComponent, pathMatch: 'full',
      },
      {
        path: 'register-student', component: RegisterStudentComponent, pathMatch: 'full'
      },
      {
        path: 'attendance', component: AttendanceComponent, pathMatch: 'prefix',
        children: [
          {
            path: 'record-attendance', component: RecordAttendanceComponent, pathMatch: 'full'
          },
          {
            path: 'attendance-history', component: AttendanceHistoryComponent, pathMatch: 'full'
          },
          {
            path: 'monthly-attendance-history', component: MonthlyAttendanceHistoryComponent, pathMatch: 'full'
          },
          {
            path: '', redirectTo: 'record-attendance', pathMatch: 'full',
          },
        ]
      },
      {
        path: 'gradebook', component: GradebookComponent, pathMatch: 'prefix',
        children: [
          {
            path: 'record-marks', component: RecordMarksComponent, pathMatch: 'full'
          },
          {
            path: 'gradebook-history', component: GradebookHistoryComponent, pathMatch: 'full'
          },
          {
            path: 'student-detail/:id', component: StudentDetailComponent, pathMatch: 'full'
          },
          {
            path: '', redirectTo: 'record-marks', pathMatch: 'full',
          },
        ]
      }
    ]
  },
  {
    path: '**', redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeacherRoutingModule { }
