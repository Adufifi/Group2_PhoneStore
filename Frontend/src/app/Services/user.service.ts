import { jwtDecode } from 'jwt-decode';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginVm } from '../Interface/LoginVm';
import { Observable, catchError, map } from 'rxjs';
import { AuthResultVm } from '../Interface/AuthResultVm';
import { User } from '../Interface/user';
import { CookieService } from 'ngx-cookie-service';
import { api_url } from '../app.config';
import { StatusResponse } from '../Interface/StatusResponse';
import { CheckRequest } from '../Interface/CheckRequest';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private cookieService: CookieService) {}

  checkAdmin(checkRequest: CheckRequest): Observable<StatusResponse> {
    return this.http.post<StatusResponse>(
      `https://localhost:7227/api/authentication/checkAdmin`,
      checkRequest,
      {
        headers: { 'Content-Type': 'application/json' },
      }
    );
  }
  getAccountId(): string {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      return '';
    }
    const decodedToken: any = jwtDecode(token);
    const userId =
      decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
    return userId;
  }
  checkUser(checkRequest: CheckRequest): Observable<StatusResponse> {
    return this.http.post<StatusResponse>(
      `https://localhost:7227/api/authentication/checkUser`,
      checkRequest,
      {
        headers: { 'Content-Type': 'application/json' },
      }
    );
  }
  login(loginVm: LoginVm): Observable<AuthResultVm> {
    return this.http.post<AuthResultVm>(`${api_url}/account/login`, loginVm);
  }

  register(registerData: RegisterRequest): Observable<any> {
    return this.http.post(`${api_url}/account/CreateAccount`, registerData);
  }

  getUserById(id: string): Observable<User> {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      throw new Error('Không tìm thấy token xác thực');
    }

    const headers = {
      Authorization: `Bearer ${token}`,
    };
    return this.http
      .get<User>(`${api_url}/account/GetById${id}`, { headers })
      .pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          throw error;
        })
      );
  }

  getAllUsers(): Observable<User[]> {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      throw new Error('Không tìm thấy token xác thực');
    }

    const headers = {
      Authorization: `Bearer ${token}`,
    };

    return this.http.get<User[]>(`${api_url}/account/GetAll`, { headers }).pipe(
      catchError((error) => {
        console.error('Error fetching users:', error);
        throw error;
      })
    );
  }
}

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}
