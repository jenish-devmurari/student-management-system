import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentComponent } from './student.component';
import { HomeComponent } from '../shared/home/home.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { GradebookComponent } from './gradebook/gradebook.component';
import { SubjectMarksComponent } from './subject-marks/subject-marks.component';
import { DashboardComponent } from '../shared/dashboard/dashboard.component';


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
            path: '', redirectTo: 'subject/1', pathMatch: 'full'
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
