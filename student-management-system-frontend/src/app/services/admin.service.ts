import { Injectable } from '@angular/core';
import { IStudent } from '../interfaces/student.interface';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { STRING_TYPE } from '@angular/compiler';
import { IResponse } from '../interfaces/response.interface';
import { ITeacher } from '../interfaces/teacher.interface';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl: string = "https://localhost:7080/api/Admin"
  constructor(private http: HttpClient, private route: Router,) { }

  public addStudent(student: IStudent): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/RegisterStudent`, JSON.stringify(student));
  }

  public addTeacher(student: ITeacher): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/RegisterTeacher`, JSON.stringify(student));
  }
}
