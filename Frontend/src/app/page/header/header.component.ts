import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

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

  constructor(private router: Router) {}

  ngOnInit() {
    // Kiểm tra xem người dùng đã đăng nhập chưa
    this.checkLoginStatus();
  }

  checkLoginStatus() {
    this.userEmail = localStorage.getItem('userEmail');
    this.isLoggedIn = !!this.userEmail;
  }

  logout() {
    // Xóa email khỏi localStorage
    localStorage.removeItem('userEmail');
    this.isLoggedIn = false;
    this.userEmail = null;
    // Chuyển hướng về trang chủ
    this.router.navigate(['/']);
  }
}
