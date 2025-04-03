import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginVm } from '../Interface/LoginVm';
import { Observable, catchError, map } from 'rxjs';
import { AuthResultVm } from '../Interface/AuthResultVm';
import { User } from '../Interface/user';
import { CookieService } from 'ngx-cookie-service';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private cookieService: CookieService) {}

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
      'Authorization': `Bearer ${token}`
    };

    console.log('Calling API:', `${api_url}/account/GetById${id}`);
    return this.http.get<User>(`${api_url}/account/GetById${id}`, { headers }).pipe(
      map(response => {
        console.log('API Response:', response);
        return response;
      }),
      catchError(error => {
        console.error('API Error:', error);
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
      'Authorization': `Bearer ${token}`
    };

    return this.http.get<User[]>(`${api_url}/account/GetAll`, { headers }).pipe(
      catchError(error => {
        console.error('Error fetching users:', error);
        throw error;
      })
    );
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete(`${api_url}/account/delete${id}`);
  }
}

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}