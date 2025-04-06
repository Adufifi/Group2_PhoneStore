import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { jwtDecode } from 'jwt-decode';
import { StatusResponse } from '../../Interface/StatusResponse';
import { api_url } from '../../app.config';

export interface User {
  id?: string;
  email: string;
  username?: string;
  fullName?: string;
  phoneNumber?: string;
  address?: string;
  avatar?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly API_URL = 'https://localhost:7107/api';
  private $user = new BehaviorSubject<User | undefined>(undefined);

  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.initializeUser();
  }
  checkAdmin(userId: string): Observable<StatusResponse> {
    return this.http.post<StatusResponse>(
      `${api_url}/authentication/checkAdmin`,
      userId
    );
  }
  private initializeUser(): void {
    const token = this.cookieService.get('Authentication');
    console.log('Token from cookie:', token);

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token.replace('Bearer ', ''));
        console.log('Decoded token:', decodedToken);

        const userId =
          decodedToken[
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'
          ];
        console.log('User ID from token:', userId);

        const userJson = localStorage.getItem('user-data');
        console.log('User data from localStorage:', userJson);

        if (userJson) {
          try {
            const userData = JSON.parse(userJson);
            userData.id = userId;
            this.$user.next(userData);
          } catch (error) {
            console.error('Lỗi khi phân tích dữ liệu người dùng:', error);
            this.clearUserData();
          }
        }
      } catch (error) {
        console.error('Lỗi khi decode token:', error);
        this.clearUserData();
      }
    }
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.API_URL}/Authentication/login-user`,
      request
    );
  }

  setUser(user: User): void {
    const token = this.cookieService.get('Authentication');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token.replace('Bearer ', ''));
        user.id = decodedToken.sid;
        console.log('Setting user with ID:', user.id);
      } catch (error) {
        console.error('Lỗi khi decode token trong setUser:', error);
      }
    }
    this.$user.next(user);
    localStorage.setItem('user-data', JSON.stringify(user));
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  getUser(): User | undefined {
    return this.$user.getValue();
  }

  getUserId(): string | undefined {
    const token = this.cookieService.get('Authentication');
    console.log('Getting token:', token);

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token.replace('Bearer ', ''));
        console.log('Decoded token in getUserId:', decodedToken);
        return decodedToken.sid;
      } catch (error) {
        console.error('Lỗi khi decode token trong getUserId:', error);
      }
    }
    return undefined;
  }

  private clearUserData(): void {
    localStorage.clear();
    this.cookieService.delete('Authentication', '/');
    this.$user.next(undefined);
  }

  logout(): void {
    this.clearUserData();
  }
}
