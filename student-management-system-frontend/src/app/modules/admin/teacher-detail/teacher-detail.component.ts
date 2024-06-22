import { Component, Input, OnDestroy, OnInit, numberAttribute } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { ITeacher } from 'src/app/interfaces/teacher.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-teacher-detail',
  templateUrl: './teacher-detail.component.html',
  styleUrls: ['./teacher-detail.component.scss']
})
export class TeacherDetailComponent implements OnInit, OnDestroy {
  public teacher!: ITeacher;
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private route: ActivatedRoute, private adminService: AdminService, private toaster: ToastrService) { }

  ngOnInit(): void {
    const sub = this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));
      if (id) {
        this.getTeacherDetailById(id);
      }
    });
    this.subscription.push(sub);
  }

  public getTeacherDetailById(id: number): void {
    const sub = this.adminService.getTeacherDetailById(id).subscribe({
      next: (res) => {
        if (res.status === HttpStatusCodes.Success) {
          this.teacher = res.data;
        } else {
          this.toaster.error(res.message)
        }
      },
      error: (err) => {
        this.toaster.error('Error fetching teacher details:', err);
      }
    });
    this.subscription.push(sub);
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }

}
