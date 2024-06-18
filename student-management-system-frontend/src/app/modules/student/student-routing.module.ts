import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentComponent } from './student.component';
import { HomeComponent } from '../shared/home/home.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { GradebookComponent } from './gradebook/gradebook.component';
import { SubjcetMarksComponent } from './subjcet-marks/subjcet-marks.component';

const routes: Routes = [
  {
    path: '', component: StudentComponent, pathMatch: 'prefix',
    children: [
      {
        path: '', redirectTo: 'home', pathMatch: 'full',
      },
      {
        path: 'home', component: HomeComponent, pathMatch: 'full',
      },
      {
        path: 'attendance', component: AttendanceComponent, pathMatch: 'full'
      },
      {
        path: 'gradebook', component: GradebookComponent, pathMatch: 'prefix',
        children: [
          {
            path: ':subject', component: SubjcetMarksComponent, pathMatch: 'full'
          },
          {
            path: '', redirectTo: 'english', pathMatch: 'full'
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
