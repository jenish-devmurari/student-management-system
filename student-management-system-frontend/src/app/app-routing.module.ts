import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './services/guards/auth.guard';
import { adminGuard } from './services/guards/admin.guard';
import { studentGuard } from './services/guards/student.guard';
import { teacherGuard } from './services/guards/teacher.guard';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: '', pathMatch: 'prefix', canActivate: [authGuard],
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'admin', pathMatch: 'prefix', canActivate: [adminGuard],
    loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule)
  },
  {
    path: 'student', pathMatch: 'prefix', canActivate: [studentGuard],
    loadChildren: () => import('./modules/student/student.module').then(m => m.StudentModule)
  },
  {
    path: 'teacher', pathMatch: 'prefix', canActivate: [teacherGuard],
    loadChildren: () => import('./modules/teacher/teacher.module').then(m => m.TeacherModule)
  },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: "enabled" })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
