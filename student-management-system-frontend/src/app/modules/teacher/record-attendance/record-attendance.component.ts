import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendanceData } from 'src/app/interfaces/attendance.interface';
import { IStudent } from 'src/app/interfaces/student.interface';
import { TeacherService } from 'src/app/services/teacher.service';

@Component({
  selector: 'app-record-attendance',
  templateUrl: './record-attendance.component.html',
  styleUrls: ['./record-attendance.component.scss']
})
export class RecordAttendanceComponent implements OnInit, OnDestroy {
  public studentList: IStudent[] = [] as IStudent[]
  public attendanceForm !: FormGroup;
  public attendanceTaken: boolean = false;
  public currentDate: string = new Date().toISOString().split('T')[0];
  private subscription: Subscription[] = [] as Subscription[];

  constructor(private teacherService: TeacherService, private toaster: ToastrService) {
    this.attendanceForm = new FormGroup({
      students: new FormArray([])
    });
  }

  ngOnInit(): void {
    this.getAllStudentOfTeacherClass();
  }

  private getAllStudentOfTeacherClass() {
    const sub = this.teacherService.getAllStudentOfTeacherClass().subscribe({
      next: (res) => {
        if (res.status == HttpStatusCodes.Success) {
          this.studentList = res.data;
          this.initializeForm();
        }
      },
      error: (err) => {
        this.toaster.error('Error fetching student list:', err);
      }
    });
    this.subscription.push(sub);
  }

  private initializeForm(): void {
    const studentsArray = this.attendanceForm.get('students') as FormArray;
    this.studentList.forEach(student => {
      studentsArray.push(new FormGroup({
        studentId: new FormControl(student.studentId),
        name: new FormControl(student.name),
        isPresent: new FormControl(false)
      }));
    });
  }

  onSubmit(): void {
    if (this.attendanceForm.valid) {
      const attendanceData: IAttendanceData = {
        date: new Date().toISOString().split('T')[0],
        attendances: this.attendanceForm.value.students.map((student: { studentId: number; isPresent: boolean; }) => ({
          studentId: student.studentId,
          isPresent: student.isPresent
        }))
      };
      const sub = this.teacherService.markAttendance(attendanceData).subscribe({
        next: (response) => {
          if (response.status == HttpStatusCodes.Success) {
            this.attendanceTaken = true;
            this.toaster.success(response.message);
          }
          else if (response.status == HttpStatusCodes.NotFound) {
            this.toaster.error(response.message);
          } else {
            this.toaster.error(response.message);
          }
        },
        error: (error) => {
          this.toaster.error('Error marking attendance:', error);
        }
      }
      );
      this.subscription.push(sub);
    }
  }

  public studentsControls(): AbstractControl[] {
    return (<FormArray>this.attendanceForm.get('students')).controls
  }

  ngOnDestroy(): void {
    if (this.subscription.length > 0) {
      this.subscription.forEach(sub => sub.unsubscribe());
    }
  }

}
