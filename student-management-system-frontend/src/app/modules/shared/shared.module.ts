import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { StudentProfileComponent } from './student-profile/student-profile.component';



@NgModule({
  declarations: [
    HomeComponent,
    StudentProfileComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    HomeComponent,
    StudentProfileComponent
  ]
})
export class SharedModule { }
