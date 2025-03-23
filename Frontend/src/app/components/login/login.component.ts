import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginData = {
    email: '',
    password: '',
  };
  rememberMe: boolean = false;
  returnUrl: string = '/home';
  errorMessage: string = '';
  isLoading: boolean = false;
  userService = inject(UserService);
  authService = inject(AuthService);
  router = inject(Router);
  cookieService = inject(CookieService);
  constructor() {}

  onSubmit() {
    this.errorMessage = '';
    this.isLoading = true;

    if (!this.loginData.email || !this.loginData.password) {
      this.errorMessage = 'Vui lòng nhập đầy đủ thông tin!';
      this.isLoading = false;
      return;
    }
    this.userService.login(this.loginData).subscribe({
      next: (res) => {
        console.log('Login response:', res);

        if (res.status === 1) {
          this.cookieService.set(
            'Authentication',
            `${res.token}`,
            undefined,
            '/',
            undefined,
            true,
            'Strict'
          );
          localStorage.setItem('email', this.loginData.email);
          this.router.navigateByUrl('/home');
        }
        if (res.status === -9999) {
          this.errorMessage = res.mess || 'An unknown error occurred.';
        }
        if (res.status === -999) {
          this.errorMessage = res.mess || 'An unknown error occurred.';
        }
        if (res.status === -2) {
          this.errorMessage = res.mess || 'An unknown error occurred.';
        }
        if (res.status === 2) {
          this.errorMessage = res.mess || 'An unknown error occurred.';
        }
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Đã xảy ra lỗi khi đăng nhập';
        this.isLoading = false;
      },
    });
  }
}
