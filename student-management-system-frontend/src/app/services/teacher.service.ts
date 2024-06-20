import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { IResponse } from '../interfaces/response.interface';
import { Observable } from 'rxjs';
import { IAttendance, IAttendanceData } from '../interfaces/attendance.interface';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private apiUrl: string = "https://localhost:7080/api/Teacher";

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
}
