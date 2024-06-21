import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { RegisterTeacherComponent } from './register-teacher/register-teacher.component';
import { RegisterComponent } from './register/register.component';
import { StudentAttendanceComponent } from './student-attendance/student-attendance.component';
import { StudentDetailComponent } from './student-detail/student-detail.component';
import { StudentGradebookComponent } from './student-gradebook/student-gradebook.component';
import { StudentListComponent } from './student-list/student-list.component';
import { TeacherDetailComponent } from './teacher-detail/teacher-detail.component';
import { TeacherListComponent } from './teacher-list/teacher-list.component';


@NgModule({
  declarations: [
    AdminComponent,
    RegisterComponent,
    RegisterTeacherComponent,
    StudentListComponent,
    StudentDetailComponent,
    StudentAttendanceComponent,
    StudentGradebookComponent,
    TeacherListComponent,
    TeacherDetailComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    ReactiveFormsModule,
    SharedModule,
    FormsModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
