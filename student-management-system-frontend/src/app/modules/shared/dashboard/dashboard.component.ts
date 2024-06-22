import { Component } from '@angular/core';
import { Roles } from 'src/app/enums/roles.enum';
import { IUser } from 'src/app/interfaces/user.interface';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  public user: IUser = {} as IUser;
  public userRole: string | null = "";
  public adminRole: Roles = Roles.Admin;
  public teacherRole: Roles = Roles.Teacher;
  public studentRole: Roles = Roles.Student;
  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole()
    this.getUserDetails()
  }

  public getUserDetails() {
    this.authService.loggedInUserDetails().subscribe(
      (res) => {
        this.user = res.data
      }
    )
  }
}
