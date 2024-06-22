import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { StudentProfileComponent } from './student-profile/student-profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '../core/core.module';
import { RegisterStudentComponent } from './register-student/register-student.component';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SortDatePipe } from 'src/app/pipes/sort-date.pipe';



@NgModule({
  declarations: [
    HomeComponent,
    RegisterStudentComponent,
    StudentProfileComponent,
    DashboardComponent,
    SortDatePipe
  ],
  imports: [
    CommonModule,
    CoreModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    HomeComponent,
    RegisterStudentComponent,
    StudentProfileComponent,
    SortDatePipe
  ]
})
export class SharedModule { }
