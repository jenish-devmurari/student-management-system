import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { StudentProfileComponent } from './student-profile/student-profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '../core/core.module';
import { RegisterStudentComponent } from './register-student/register-student.component';



@NgModule({
  declarations: [
    HomeComponent,
    RegisterStudentComponent,
    StudentProfileComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    HomeComponent,
    RegisterStudentComponent,
    StudentProfileComponent
  ]
})
export class SharedModule { }
