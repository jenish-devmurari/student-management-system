import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Classes } from 'src/app/enums/classes.enum';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { Roles } from 'src/app/enums/roles.enum';
import { Subjects } from 'src/app/enums/subjects.enum';
import { IUser } from 'src/app/interfaces/user.interface';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public user: IUser = {} as IUser;
  public userRole: string | null = "";
  public adminRole: Roles = Roles.Admin;
  public teacherRole: Roles = Roles.Teacher;
  public studentRole: Roles = Roles.Student;
  constructor(private authService: AuthService, private toaster: ToastrService) {
  }

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole()
    this.getUserDetails()
  }

  private getUserDetails() {
    this.authService.loggedInUserDetails().subscribe({
      next: (res) => {
        if (res.status === HttpStatusCodes.Success) {
          this.user = res.data
        } else {
          this.toaster.success(res.message);
        }
      },
      error: (err) => {
        this.toaster.error(err)
      }
    });
  }

  getSubjectName(subjectId: number | undefined): string {
    return subjectId ? Subjects[subjectId] : 'Unknown Subject';
  }

  getClassName(classId: number | undefined): string {
    return classId ? Classes[classId] : 'Unknown Class';
  }
}
