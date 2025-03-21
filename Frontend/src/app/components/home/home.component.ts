import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="home-container">
      <h1>Trang chủ</h1>
      <p>Xin chào, {{ userEmail }}!</p>
      <button (click)="logout()">Đăng xuất</button>
    </div>
  `,
  styles: [
    `
      .home-container {
        padding: 20px;
        max-width: 800px;
        margin: 0 auto;
      }
      button {
        padding: 10px 20px;
        background-color: #667eea;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
      }
      button:hover {
        background-color: #764ba2;
      }
    `,
  ],
})
export class HomeComponent {
  userEmail: string = '';

  constructor(private authService: AuthService, private router: Router) {
    const user = this.authService.getUser();
    this.userEmail = user?.email || 'Người dùng';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
