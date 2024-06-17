import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = sessionStorage.getItem('authToken');
    const headersConfig: any = {
      'Content-Type': 'application/json'
    };

    if (token) {
      headersConfig['Authorization'] = `Bearer ${token}`;
    }

    const modifiedRequest = request.clone({
      setHeaders: headersConfig
    });

    return next.handle(modifiedRequest);
  }
}