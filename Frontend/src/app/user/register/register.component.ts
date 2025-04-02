import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RegisterRequest, UserService } from '../../Services/user.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  registerService = inject(UserService);
  router = inject(Router);

  user = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };

  errors: string[] = [];
  isSuccess = false;

  register() {
    this.errors = [];
    this.isSuccess = false;
    if (this.user.password !== this.user.confirmPassword) {
      this.errors.push('Mật khẩu xác nhận không khớp');
      return;
    }
    if (this.user.password.length < 6) {
      this.errors.push('Mật khẩu phải có ít nhất 6 ký tự');
      return;
    }
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailPattern.test(this.user.email)) {
      this.errors.push('Email không hợp lệ');
      return;
    }
    const registerData: RegisterRequest = {
      username: this.user.username,
      email: this.user.email,
      password: this.user.password,
    };

    console.log('Attempting registration with data:', {
      ...registerData,
      password: '[REDACTED]',
    });
    this.registerService.register(registerData).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        this.isSuccess = true;
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Registration error:', error);
        if (error.error && error.error.mess) {
          this.errors.push(error.error.mess);
        } else if (error.error && error.error.errors) {
          // Debug
          Object.values(error.error.errors).forEach((err: any) => {
            this.errors.push(err[0]);
          });
        } else if (error.status === 0) {
          this.errors.push(
            'Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối mạng và thử lại.'
          );
        } else if (error.status === 400) {
          this.errors.push(
            'Dữ liệu đăng ký không hợp lệ. Vui lòng kiểm tra lại thông tin.'
          );
        } else {
          this.errors.push(
            `Có lỗi xảy ra khi đăng ký (${error.status}). Vui lòng thử lại sau.`
          );
        }
      },
    });
  }
}