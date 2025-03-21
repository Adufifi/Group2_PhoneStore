import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface RegisterRequest {
  email: string;
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private apiUrl = `${environment.api_url}/api/auth/register`;

  constructor(private http: HttpClient) {}

  register(registerData: RegisterRequest): Observable<any> {
    return this.http.post(this.apiUrl, registerData);
  }
}
