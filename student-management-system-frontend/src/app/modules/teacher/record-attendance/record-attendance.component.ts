import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { HttpStatusCodes } from 'src/app/enums/http-status-code.enum';
import { IAttendanceData } from 'src/app/interfaces/attendance.interface';
import { IStudent } from 'src/app/interfaces/student.interface';
import { TeacherService } from 'src/app/services/teacher.service';

@Component({
  selector: 'app-record-attendance',
  templateUrl: './record-attendance.component.html',
  styleUrls: ['./record-attendance.component.scss']
})
export class RecordAttendanceComponent implements OnInit {
  public studentList: IStudent[] = [] as IStudent[]
  public attendanceForm !: FormGroup;
  public attendanceTaken: boolean = false;
  public currentDate: string = new Date().toISOString().split('T')[0];

  constructor(private teacherService: TeacherService, private toaster: ToastrService) {
    this.attendanceForm = new FormGroup({
      students: new FormArray([])
    });
  }

  ngOnInit(): void {
    this.getAllStudentOfTeacherClass();
  }

  private getAllStudentOfTeacherClass() {
    this.teacherService.getAllStudentOfTeacherClass().subscribe({
      next: (res) => {
        if (res.status == HttpStatusCodes.Success) {
          this.studentList = res.data;
          this.initializeForm();
        }
      },
      error: (err) => {
        console.error('Error fetching student list:', err);
      }
    });
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
      this.teacherService.markAttendance(attendanceData).subscribe(
        (response) => {
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
        (error) => {
          console.error('Error marking attendance:', error);
        }
      );
    }
  }

  public studentsControls(): AbstractControl[] {
    return (<FormArray>this.attendanceForm.get('students')).controls
  }


}
