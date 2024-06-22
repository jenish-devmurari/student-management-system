import {
  HttpErrorResponse,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { HttpStatusCodes } from '../enums/http-status-code.enum';
import { SessionStorageService } from '../services/session-storage.service';
import { Router } from '@angular/router';

@Injectable()
export class GlobalErrorInterceptor implements HttpInterceptor {

  constructor(private sessionStorage: SessionStorageService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
        switch (error.status) {
          case HttpStatusCodes.InternalServerError:
            errorMessage = "Internal server error";
            break;
          case HttpStatusCodes.BadRequest:
            errorMessage = "Bad request from client side";
            break;
          case HttpStatusCodes.Unauthorized:
            errorMessage = "Unauthorized";
            this.sessionStorage.removeItem('authToken');
            this.router.navigate(['auth/login']);
            break;
          case HttpStatusCodes.NotFound:
            errorMessage = "Resource not found";
            break;
          default:
            errorMessage = "Something went wrong, please try later";
            break;
        }
        return throwError(() => errorMessage);
      })
    );
  }
}
