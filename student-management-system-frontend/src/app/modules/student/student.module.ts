import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentRoutingModule } from './student-routing.module';
import { StudentComponent } from './student.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AttendanceComponent } from './attendance/attendance.component';
import { GradebookComponent } from './gradebook/gradebook.component';
import { SubjectMarksComponent } from './subject-marks/subject-marks.component';



@NgModule({
  declarations: [
    StudentComponent,
    AttendanceComponent,
    GradebookComponent,
    SubjectMarksComponent,
  ],
  imports: [
    CommonModule,
    CoreModule,
    SharedModule,
    StudentRoutingModule
  ]
})
export class StudentModule { }
