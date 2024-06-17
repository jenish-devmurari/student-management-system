import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AuthComponent } from './auth.component';


const routes: Routes = [
  {
    path: '', component: AuthComponent, pathMatch: 'prefix',
    children: [
      {
        path: 'login', component: LoginComponent, pathMatch: 'full'
      },
      {
        path: 'change-password', component: ChangePasswordComponent, pathMatch: 'full'
      }
    ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
