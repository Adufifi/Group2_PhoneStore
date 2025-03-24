import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  user = {
    fullname: '',
    email: '',
    phone: '',
    password: '',
    confirmPassword: '',
  };

  errors: string[] = [];
  isSuccess = false;

  constructor(private router: Router) {}

  register() {
    this.errors = [];
    this.isSuccess = false;

    // Kiểm tra xác nhận mật khẩu
    if (this.user.password !== this.user.confirmPassword) {
      this.errors.push('Mật khẩu xác nhận không khớp');
      return;
    }

    // Kiểm tra độ dài mật khẩu
    if (this.user.password.length < 6) {
      this.errors.push('Mật khẩu phải có ít nhất 6 ký tự');
      return;
    }

    // Kiểm tra email hợp lệ
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailPattern.test(this.user.email)) {
      this.errors.push('Email không hợp lệ');
      return;
    }

    // Kiểm tra số điện thoại
    const phonePattern = /^[0-9]{10}$/;
    if (!phonePattern.test(this.user.phone)) {
      this.errors.push('Số điện thoại phải có 10 chữ số');
      return;
    }

    // Đăng ký thành công
    console.log('Registration successful:', this.user);
    this.isSuccess = true;

    // Chuyển đến trang đăng nhập sau 2 giây
    setTimeout(() => {
      this.router.navigate(['/login']);
    }, 2000);
  }
}
