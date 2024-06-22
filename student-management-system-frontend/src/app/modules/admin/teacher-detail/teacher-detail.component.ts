import { Component, Input, OnInit, numberAttribute } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { ITeacher } from 'src/app/interfaces/teacher.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-teacher-detail',
  templateUrl: './teacher-detail.component.html',
  styleUrls: ['./teacher-detail.component.scss']
})
export class TeacherDetailComponent implements OnInit {
  public teacher!: ITeacher;

  constructor(private route: ActivatedRoute, private adminService: AdminService, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));
      if (id) {
        this.getTeacherDetailById(id);
      }
    });
  }

  public getTeacherDetailById(id: number): void {
    this.adminService.getTeacherDetailById(id).subscribe({
      next: (res) => {
        if (res.status === HttpStatusCodes.Success) {
          this.teacher = res.data;
          console.log(this.teacher)
        } else {
          this.toaster.error(res.message)
        }
      },
      error: (err) => {
        this.toaster.error('Error fetching teacher details:', err);
      }
    });
  }
}
