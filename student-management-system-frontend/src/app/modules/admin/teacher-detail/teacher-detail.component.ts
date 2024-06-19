import { Component, Input, OnInit, numberAttribute } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ITeacher } from 'src/app/interfaces/teacher.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-teacher-detail',
  templateUrl: './teacher-detail.component.html',
  styleUrls: ['./teacher-detail.component.scss']
})
export class TeacherDetailComponent implements OnInit {
  public teacher!: ITeacher;

  constructor(private route: ActivatedRoute, private adminService: AdminService) { }

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
        if (res.status === 200) {
          this.teacher = res.data;
          console.log(this.teacher)
        } else {
          console.error(res.message);
        }
      },
      error: (err) => {
        console.error('Error fetching teacher details:', err);
      }
    });
  }
}
