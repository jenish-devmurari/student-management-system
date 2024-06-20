import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { IAttendance } from '../interfaces/attendance.interface';
import { IGradebook } from '../interfaces/gradebook.interface';
import { IResponse } from '../interfaces/response.interface';
import { IStudent } from '../interfaces/student.interface';
import { ITeacher } from '../interfaces/teacher.interface';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private studentDataSubject: BehaviorSubject<IStudent[]> = new BehaviorSubject<IStudent[]>([]);
  public studentData$: Observable<IStudent[]> = this.studentDataSubject.asObservable();

  private teacherDataSubject: BehaviorSubject<ITeacher[]> = new BehaviorSubject<ITeacher[]>([]);
  public teacherData$: Observable<ITeacher[]> = this.teacherDataSubject.asObservable();

  private attendanceDataSubject: BehaviorSubject<IAttendance[]> = new BehaviorSubject<IAttendance[]>([]);
  public attendanceData$: Observable<IAttendance[]> = this.attendanceDataSubject.asObservable();

  private gradebookDataSubject: BehaviorSubject<IGradebook[]> = new BehaviorSubject<IGradebook[]>([]);
  public gradebookData$: Observable<IGradebook[]> = this.gradebookDataSubject.asObservable();

  private apiUrl: string = "https://localhost:7080/api/Admin"
  constructor(private http: HttpClient, private route: Router,) { }

  public addStudent(student: IStudent): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/RegisterStudent`, JSON.stringify(student));
  }

  public getAllStudent(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetAllStudent`).pipe(
      tap(response => {
        if (response.status === 200) {
          this.studentDataSubject.next(response.data);
        }
      }),
    );
  }

  public getStudentDetailById(id: number): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentById/${id}`);
  }

  public updateStudent(student: IStudent, id: number | undefined): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.apiUrl}/UpdateStudent/${id}`, student)
  }

  public deleteStudent(id: number | undefined): Observable<IResponse> {
    return this.http.delete<IResponse>(`${this.apiUrl}/DeleteStudent/${id}`)
  }


  public addTeacher(teacher: ITeacher): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/RegisterTeacher`, JSON.stringify(teacher));
  }


  public getAllTeacher(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetAllTeacher`).pipe(
      tap(response => {
        if (response.status === 200) {
          this.teacherDataSubject.next(response.data);
        }
      }),
    );
  }

  public getTeacherDetailById(id: number | undefined): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetTeacherById/${id}`);
  }

  public updateTeacher(teacher: ITeacher, id: number | undefined): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.apiUrl}/UpdateTeacher/${id}`, teacher)
  }

  public deleteTeacher(id: number | undefined): Observable<IResponse> {
    return this.http.delete<IResponse>(`${this.apiUrl}/DeleteTeacher/${id}`)
  }

  public getStudentAttendanceDetail(id: number): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentAttendanceDetails/${id}`)
  }

  public getStudentGradeBookDetail(id: number): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentGradesDetails/${id}`)
  }

  public updateAttendance(attendance: IAttendance): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.apiUrl}/UpdateStudentAttendanceDetails`, attendance)
  }


  public updateGrades(grades: IGradebook): Observable<IResponse> {
    console.log(grades.marks);
    console.log(grades.totalMarks);
    return this.http.put<IResponse>(`${this.apiUrl}/UpdateStudentGradesDetails`, grades)
  }

}
