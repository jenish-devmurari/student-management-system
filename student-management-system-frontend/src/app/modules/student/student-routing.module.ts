import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from '../shared/dashboard/dashboard.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { GradebookComponent } from './gradebook/gradebook.component';
import { StudentComponent } from './student.component';
import { SubjectMarksComponent } from './subject-marks/subject-marks.component';


const routes: Routes = [
  {
    path: '', component: StudentComponent, pathMatch: 'prefix',
    children: [
      {
        path: '', redirectTo: 'dashboard', pathMatch: 'full',
      },
      {
        path: 'dashboard', component: DashboardComponent, pathMatch: 'full',
      },
      {
        path: 'attendance', component: AttendanceComponent, pathMatch: 'full'
      },
      {
        path: 'gradebook', component: GradebookComponent, pathMatch: 'prefix',
        children: [
          {
            path: 'subject/:subjectId', component: SubjectMarksComponent, pathMatch: 'full'
          },
          {
            path: '', redirectTo: 'subject/2', pathMatch: 'full'
          }
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
export class StudentRoutingModule { }
