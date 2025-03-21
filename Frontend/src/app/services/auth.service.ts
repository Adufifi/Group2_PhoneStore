import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private user: any = null;

  constructor(private cookieService: CookieService) {
    this.loadUser();
  }

  loadUser() {
    const token = this.getAuthToken();
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        this.user = {
          email: decodedToken.email,
          role: decodedToken.role,
        };
      } catch (error) {
        console.error('Error decoding token:', error);
        this.logout();
      }
    }
  }

  getUser() {
    return this.user;
  }

  logout() {
    this.user = null;
    this.clearAuthTokens();
  }

  setAuthTokens(token: string, refreshToken: string) {
    try {
      // Xóa cookie cũ
      this.clearAuthTokens();

      // Tính thời gian hết hạn
      const expires = new Date();
      expires.setDate(expires.getDate() + 1); // Token hết hạn sau 1 ngày

      const refreshExpires = new Date();
      refreshExpires.setDate(refreshExpires.getDate() + 7); // Refresh token hết hạn sau 7 ngày

      // Set cookie mới
      this.cookieService.set('Authentication', token, {
        expires,
        path: '/',
        secure: false, // Set true in production
        sameSite: 'Lax',
      });

      this.cookieService.set('RefreshToken', refreshToken, {
        expires: refreshExpires,
        path: '/',
        secure: false, // Set true in production
        sameSite: 'Lax',
      });

      // Load user info from token
      this.loadUser();

      return true;
    } catch (error) {
      console.error('Error setting auth tokens:', error);
      return false;
    }
  }

  getAuthToken(): string {
    return this.cookieService.get('Authentication') || '';
  }

  getRefreshToken(): string {
    return this.cookieService.get('RefreshToken') || '';
  }

  clearAuthTokens() {
    this.cookieService.delete('Authentication', '/');
    this.cookieService.delete('RefreshToken', '/');
  }

  isAuthenticated(): boolean {
    return !!this.getAuthToken();
  }
}
