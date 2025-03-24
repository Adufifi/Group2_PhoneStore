import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
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
    this.loggedIn.next(false);
  }
}
