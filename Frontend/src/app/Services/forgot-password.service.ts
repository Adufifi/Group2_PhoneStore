import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class ForgotPasswordService {
  constructor(private http: HttpClient) {}

  sendOtp(email: string): Observable<string> {
    return this.http.post(
      `${api_url}/account/forgot-password`,
      JSON.stringify(email),
      {
        headers: { 'Content-Type': 'application/json' },
        responseType: 'text',
      }
    ) as Observable<string>;
  }
  // Xác thực OTP
  verifyOtp(email: string, otp: string): Observable<any> {
    const requestData = { email, otp };
    return this.http.post<any>(`${api_url}/auth/verify-otp`, requestData, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  // Đặt lại mật khẩu
  resetPassword(email: string, newPassword: string): Observable<any> {
    const requestData = { email, newPassword };
    return this.http.post<any>(`${api_url}/auth/reset-password`, requestData, {
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
