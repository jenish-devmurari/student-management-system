import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeacherRoutingModule } from './teacher-routing.module';
import { TeacherComponent } from './teacher.component';
import { CoreModule } from '../core/core.module';


@NgModule({
  declarations: [
    TeacherComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    TeacherRoutingModule
  ]
})
export class TeacherModule { }
