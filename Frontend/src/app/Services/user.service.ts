import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginVm } from '../Interface/LoginVm';
import { Observable } from 'rxjs';
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
    const headers = {
      'Authorization': `Bearer ${token}`
    };
    return this.http.get<Customer>(`${api_url}/User/GetById/${id}`, { headers });
  }
}

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}