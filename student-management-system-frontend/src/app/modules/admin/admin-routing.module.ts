import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { HomeComponent } from '../shared/home/home.component';
import { RegisterComponent } from './register/register.component';
import { RegisterTeacherComponent } from './register-teacher/register-teacher.component';
import { StudentListComponent } from './student-list/student-list.component';
import { StudentDetailComponent } from './student-detail/student-detail.component';
import { StudentAttendanceComponent } from './student-attendance/student-attendance.component';
import { StudentGradebookComponent } from './student-gradebook/student-gradebook.component';
import { TeacherDetailComponent } from './teacher-detail/teacher-detail.component';
import { TeacherListComponent } from './teacher-list/teacher-list.component';
import { RegisterStudentComponent } from '../shared/register-student/register-student.component';

const routes: Routes = [
  {
    path: '', component: AdminComponent, pathMatch: 'prefix',
    children: [
      {
        path: '', redirectTo: 'register', pathMatch: 'full',
      },
      {
        path: 'home', component: HomeComponent, pathMatch: 'full',
      },
      {
        path: 'register', component: RegisterComponent, pathMatch: 'prefix',
        children: [
          {
            path: 'teacher', component: RegisterTeacherComponent, pathMatch: 'full'
          },
          {
            path: 'student', component: RegisterStudentComponent, pathMatch: 'full'
          },
          {
            path: '', redirectTo: 'student', pathMatch: 'full'
          }
        ]
      },
      {
        path: 'student', component: StudentListComponent, pathMatch: 'full',
      },
      {
        path: 'student/student-detail/:id', component: StudentDetailComponent, pathMatch: 'prefix',
        children: [
          {
            path: 'attendance', component: StudentAttendanceComponent, pathMatch: 'full'
          },
          {
            path: 'gradebook', component: StudentGradebookComponent, pathMatch: 'full'
          },
          {
            path: '', redirectTo: 'attendance', pathMatch: 'full'
          }
        ]
      },
      {
        path: 'teacher', component: TeacherListComponent, pathMatch: 'full'
      },
      {
        path: 'teacher/teacher-detail/:id', component: TeacherDetailComponent, pathMatch: 'full',
      }
    ]
  },
  {
    path: "**", redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
