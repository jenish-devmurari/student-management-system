import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { RegisterComponent } from './register/register.component';
import { RegisterTeacherComponent } from './register-teacher/register-teacher.component';
import { RegisterStudentComponent } from './register-student/register-student.component';
import { StudentListComponent } from './student-list/student-list.component';
import { StudentDetailComponent } from './student-detail/student-detail.component';
import { StudentAttendanceComponent } from './student-attendance/student-attendance.component';
import { StudentGradebookComponent } from './student-gradebook/student-gradebook.component';


@NgModule({
  declarations: [
    AdminComponent,
    RegisterComponent,
    RegisterTeacherComponent,
    RegisterStudentComponent,
    StudentListComponent,
    StudentDetailComponent,
    StudentAttendanceComponent,
    StudentGradebookComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
