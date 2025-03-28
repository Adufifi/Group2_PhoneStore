import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginVm } from '../Interface/LoginVm';
import { Observable } from 'rxjs';
import { AuthResultVm } from '../Models/AuthResultVm';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}
  login(loginVm: LoginVm): Observable<AuthResultVm> {
    return this.http.post<AuthResultVm>(`${api_url}/account/login`, loginVm);
  }
  register(registerData: RegisterRequest): Observable<any> {
    return this.http.post(`${api_url}/account/CreateAccount`, registerData);
  }
}
export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}
