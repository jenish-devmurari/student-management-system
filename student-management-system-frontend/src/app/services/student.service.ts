import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IResponse } from '../interfaces/response.interface';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private apiUrl: string = "https://localhost:7080/api/Student"
  constructor(private http: HttpClient, private route: Router,) { }

  public getAttendanceOfStudent(): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetAllAttendence`);
  }

  public getStudentGradesBasedOnSubject(subjectId: number): Observable<IResponse> {
    return this.http.get<IResponse>(`${this.apiUrl}/GetStudentGradesDetailsBasedOnSubject/${subjectId}`);
  }
}
