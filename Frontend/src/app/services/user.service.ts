import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginVm, AuthResultVm } from '../models/auth.model';
import { environment } from '../../environments/environment';
import { StatusResponse } from '../Models/StatusResponse';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = environment.api_url;

  constructor(private http: HttpClient) {}

  login(loginData: LoginVm): Observable<AuthResultVm> {
    return this.http.post<AuthResultVm>(
      `${this.apiUrl}/Account/login`,
      loginData
    );
  }

  register(registerData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Account/register`, registerData);
  }

  forgotPassword(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Account/forgot-password`, { email });
  }

  resetPassword(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Account/reset-password`, data);
  }
}

export const reusltLogin: AuthResultVm | StatusResponse | undefined = undefined;
