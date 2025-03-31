import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../Services/auth.service';

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

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit() {
    // Kiểm tra trạng thái đăng nhập khi component được khởi tạo
    this.checkLoginStatus();

    // Đăng ký theo dõi thay đổi trạng thái đăng nhập
    this.authService.isLoggedIn.subscribe((status: boolean) => {
      this.isLoggedIn = status;
      if (status) {
        this.userEmail = localStorage.getItem('email');
        this.userId = localStorage.getItem('userId');
      } else {
        this.userEmail = null;
        this.userId = null;
      }
    });
  }

  private checkLoginStatus() {
    // Kiểm tra token trong localStorage
    const token = localStorage.getItem('token');
    if (token) {
      this.isLoggedIn = true;
      this.userEmail = localStorage.getItem('email');
      this.userId = localStorage.getItem('userId');
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
