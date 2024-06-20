import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Roles } from 'src/app/enums/roles.enum';
import { AuthService } from 'src/app/services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  public userRole: string | null = "";
  public isLoggedIn !: boolean;
  public adminRole: Roles = Roles.Admin;
  public teacherRole: Roles = Roles.Teacher;
  public studentRole: Roles = Roles.Student;

  constructor(private authService: AuthService, private route: Router) {
  }

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole()
    this.isLoggedIn = this.authService.isLoggedIn();
  }

  public logout() {
    Swal.fire({
      title: 'Are you sure?',
      text: "Do you really want to log out?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, log out!',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        this.authService.logoutAfterChangePassword()
        Swal.fire(
          'Logged out!',
          'You have been logged out.',
          'success'
        );
        this.isLoggedIn = false;
        this.userRole = null;
      }
    });
  }

}
