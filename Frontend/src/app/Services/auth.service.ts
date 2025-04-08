import { inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable } from 'rxjs';
import { api_url } from '../app.config';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  cookieServices = inject(CookieService);
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());
  public isLoggedIn = this.loggedIn.asObservable();
  http = inject(HttpClient);
  private hasToken(): boolean {
    return !!localStorage.getItem('email');
  }
  getCurrentUserId(): Promise<string | null> {
    const email = localStorage.getItem('email');
    if (!email) return Promise.resolve(null);

    return new Promise((resolve) => {
      this.http
        .get<string>(`${api_url}/account/GetUserIdByEmail?email=${email}`)
        .subscribe({
          next: (userId) => {
            resolve(userId);
          },
          error: (error) => {
            console.error('Error getting user ID:', error);
            resolve(null);
          },
        });
    });
  }

  login(email: string) {
    localStorage.setItem('email', email);
    this.loggedIn.next(true);
  }

  logout() {
    localStorage.removeItem('email');
    this.cookieServices.delete('Authentication', '/');
    this.loggedIn.next(false);
  }
}
