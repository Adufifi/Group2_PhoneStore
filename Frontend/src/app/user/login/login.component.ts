import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  user = {
    email: '',
    password: '',
  };

  errorMessage = '';

  constructor(private router: Router) {}

  login() {
    console.log('Login attempt:', this.user);

    // Kiểm tra đăng nhập với tài khoản admin
    if (this.user.email === 'admin' && this.user.password === '123') {
      console.log('Admin login successful');
      // Lưu email vào localStorage
      localStorage.setItem('userEmail', this.user.email);
      this.router.navigate(['/admin']);
    } else if (this.user.email && this.user.password) {
      // Mô phỏng đăng nhập thành công cho người dùng thông thường
      console.log('User login successful');
      // Lưu email vào localStorage
      localStorage.setItem('userEmail', this.user.email);
      this.router.navigate(['/']);
    } else {
      // Hiển thị thông báo lỗi nếu thông tin đăng nhập không chính xác
      this.errorMessage = 'Email hoặc mật khẩu không chính xác';
    }
  }
}
