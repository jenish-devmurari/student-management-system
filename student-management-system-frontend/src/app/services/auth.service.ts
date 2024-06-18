import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILogin } from 'src/app/interfaces/login.interface';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { IResponse } from '../interfaces/response.interface';
import { Router } from '@angular/router';
import * as jwt_decode from "jwt-decode";
import { SessionStorageService } from './session-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl: string = "https://localhost:7080/api/User"
  private authToken: string = "";


  constructor(private http: HttpClient, private route: Router, private sessionStorageService: SessionStorageService) { }

  public login(loginData: ILogin): Observable<IResponse> {
    return this.http.post<IResponse>(`${this.apiUrl}/Login`, loginData)
      .pipe(
        tap((response: IResponse) => {
          if (response.data) {
            const responseData = response.data;
            if (responseData['token']) {
              this.authToken = responseData['token'];
              if (responseData['isPasswordReset'] == false) {
                this.route.navigate(['change-password'])
              } else {
                this.sessionStorageService.setItem('authToken', this.authToken);
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
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.authToken}`
      })
    };
    return this.http.put<IResponse>(`${this.apiUrl}/ChangePassword`, JSON.stringify(confirmPassword), httpOptions);
  }

  public logoutAfterChangePassword(): void {
    this.sessionStorageService.removeItem('authToken');
    this.route.navigate(['login']);
  }

  public getToken(): string | null {
    return this.sessionStorageService.getItem('authToken');
  }


  public getUserRole(): string | null {
    const token = this.getToken();
    if (token) {
      return this.extractRoleFromToken(token);
    }
    return null;
  }

  private extractRoleFromToken(token: string): string {
    const decodedToken: any = jwt_decode.jwtDecode(token);
    return decodedToken.role;
  }

  public isLoggedIn(): boolean {
    const token: string | null = this.sessionStorageService.getItem('authToken');
    if (token) {
      return true;
    }
    return false
  }


}


