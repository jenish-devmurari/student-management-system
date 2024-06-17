import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILogin } from 'src/app/interfaces/login.interface';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { IResponse } from '../interfaces/response.interface';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl: string = "https://localhost:7080/api/User"
  private authToken: string = ""
  constructor(private http: HttpClient, private route: Router) { }

  public login(loginData: ILogin): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/Login`, loginData)
      .pipe(
        tap((response: IResponse) => {
          if (response.data) {
            const responseData = response.data;
            if (responseData['token']) {
              this.authToken = responseData['token'];
              sessionStorage.setItem('authToken', this.authToken);
              if (responseData['isPasswordReset'] == false) {
                this.route.navigate(['change-password'])
              }
            }
          }
        }),
        catchError((error) => {
          return throwError(() => new Error(error));
        })
      );
  }

  public changePassword(confirmPassword: string): Observable<IResponse> {
    return this.http.put<IResponse>(`${this.apiUrl}/ChangePassword`, JSON.stringify(confirmPassword));
  }

  public logoutAfterChangePassword(): void {
    sessionStorage.removeItem('authToken');
    this.route.navigate(['login']);
  }

  public getToken(): string | null {
    return sessionStorage.getItem('authToken');
  }

}
