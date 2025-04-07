import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginVm } from '../Interface/LoginVm';
import { Observable, catchError, map, forkJoin } from 'rxjs';
import { AuthResultVm } from '../Interface/AuthResultVm';
import { Customer } from '../Interface/customer.interface';
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

  getUserById(id: string): Observable<Customer> {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      throw new Error('Không tìm thấy token xác thực');
    }

    const headers = {
      'Authorization': `Bearer ${token}`
    };

    console.log('Calling API:', `${api_url}/account/GetById${id}`);
    return this.http.get<Customer>(`${api_url}/account/GetById${id}`, { headers }).pipe(
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

  getAllUsers(): Observable<Customer[]> {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      throw new Error('Không tìm thấy token xác thực');
    }

    const headers = {
      'Authorization': `Bearer ${token}`
    };

    return forkJoin({
      users: this.http.get<any[]>(`${api_url}/account/GetAll`, { headers }),
      roles: this.http.get<any[]>(`${api_url}/role/All`, { headers })
    }).pipe(
      map(({ users, roles }) => {
        return users.map(user => {
          const userRole = roles.find(role => role.id === user.roleId);
          return {
            ...user,
            roleName: userRole ? userRole.roleName : 'Chưa xác định'
          };
        });
      }),
      catchError(error => {
        console.error('Error fetching data:', error);
        throw error;
      })
    );
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete(`${api_url}/account/delete/${id}`);
  }

  updateUser(user: Customer): Observable<any> {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      throw new Error('Không tìm thấy token xác thực');
    }

    const headers = {
      'Authorization': `Bearer ${token}`
    };

    return this.http.post(`${api_url}/account/update-profile`, {
      email: user.email,
      userName: user.userName,
      roleId: user.role_id,
      createdDate: user.created_date
    }, { headers });
  }
}

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}