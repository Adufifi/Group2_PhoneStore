import { inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  cookieServices = inject(CookieService);
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());
  public isLoggedIn = this.loggedIn.asObservable();

  private hasToken(): boolean {
    return !!localStorage.getItem('email');
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
