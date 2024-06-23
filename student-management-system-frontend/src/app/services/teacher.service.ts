import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { IResponse } from '../interfaces/response.interface';
import { Observable } from 'rxjs';
import { IAttendance, IAttendanceData } from '../interfaces/attendance.interface';
import { IStudent } from '../interfaces/student.interface';
import { IGradebook } from '../interfaces/gradebook.interface';
import { TeacherApi } from '../constants/constants';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private apiUrl: string = TeacherApi;

  constructor(private http: HttpClient, private route: Router,) { }

  public getAllStudentOfTeacherClass(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetAllStudentOfTeacherClass`);
  }

  public markAttendance(attendanceData: IAttendanceData): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/MarkAttendance`, JSON.stringify(attendanceData));
  }

  public getAttendanceHistory(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/AttendanceHistory`);
  }

  public updateAttendance(attendance: IAttendance): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.apiUrl}/EditAttendanceHistory/${attendance.id}`, JSON.stringify(attendance));
  }

  public addStudent(student: IStudent): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/RegisterStudent`, JSON.stringify(student));
  }

  public marksAdd(grade: IGradebook): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/AddMarks`, JSON.stringify(grade));
  }

  public getAllGradesDetails(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetAllStudentGrades`);
  }

  public updateGrades(grades: IGradebook): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.apiUrl}/UpdateMarksOfStudent`, grades)
  }

  public getStudentDetailById(id: number): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentDetailById/${id}`)
  }

  public getStudentGradesDetailById(id: number): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentGradesDetailById/${id}`)
  }

  public getAttendanceBasedOnDate(date: string): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentAttendanceDetailByDate/${date}`)
  }

  public getStudentEmailListBasedOnSearch(query: string): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentEmailList/${query}`)
  }
}