import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import {
  RegisterService,
  RegisterRequest,
} from '../../services/register.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registerData = {
    email: '',
    username: '',
    password: '',
    confirmPassword: '',
  };

  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private router: Router,
    private registerService: RegisterService
  ) {}

  isPasswordMatch(): boolean {
    return this.registerData.password === this.registerData.confirmPassword;
  }

  onSubmit() {
    if (
      this.registerData.email &&
      this.registerData.username &&
      this.registerData.password &&
      this.isPasswordMatch()
    ) {
      this.isLoading = true;
      this.errorMessage = '';

      const registerRequest: RegisterRequest = {
        email: this.registerData.email,
        username: this.registerData.username,
        password: this.registerData.password,
      };

      this.registerService.register(registerRequest).subscribe({
        next: (response) => {
          this.isLoading = false;
          alert('Đăng ký thành công!');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage =
            error.error.message || 'Có lỗi xảy ra khi đăng ký.';
        },
      });
    } else {
      this.errorMessage =
        'Vui lòng điền đầy đủ thông tin và đảm bảo mật khẩu khớp.';
    }
  }
}
