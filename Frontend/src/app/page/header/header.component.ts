import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../Services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  standalone: true,
})
export class HeaderComponent implements OnInit {
  isLoggedIn = false;
  userEmail: string | null = null;
  userId: string | null = null;

  constructor(
    private router: Router,
    private authService: AuthService,
    private cookieService: CookieService
  ) {}

  ngOnInit() {
    this.checkLoginStatus();
    this.authService.isLoggedIn.subscribe((status: boolean) => {
      this.isLoggedIn = status;
      if (status) {
        this.userEmail = localStorage.getItem('email');
        this.getUserIdFromToken();
      } else {
        this.userEmail = null;
        this.userId = null;
      }
    });
  }

  private checkLoginStatus() {
    const token = this.cookieService.get('Authentication');
    if (token) {
      this.isLoggedIn = true;
      this.userEmail = localStorage.getItem('email');
      this.getUserIdFromToken();
    }
  }

  private getUserIdFromToken() {
    const token = this.cookieService.get('Authentication');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token.replace('Bearer ', ''));
        const sidKey =
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid';
        this.userId = decodedToken[sidKey];
        console.log('User ID from token:', this.userId);
      } catch (error) {
        console.error('Lỗi khi lấy ID từ token:', error);
        this.userId = null;
      }
    }
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.userEmail = null;
    this.userId = null;
    this.router.navigate(['/']);
  }
}
