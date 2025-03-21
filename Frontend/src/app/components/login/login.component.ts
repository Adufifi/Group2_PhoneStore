import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { AuthResultVm } from '../../models/auth.model';

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

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/home']);
    }

    this.route.queryParams.subscribe((params) => {
      this.returnUrl = params['returnUrl'] || '/home';
    });
  }

  onSubmit() {
    debugger;
    this.errorMessage = '';
    this.isLoading = true;

    if (!this.loginData.email || !this.loginData.password) {
      this.errorMessage = 'Vui lòng nhập đầy đủ thông tin!';
      this.isLoading = false;
      return;
    }

    this.userService.login(this.loginData).subscribe({
      next: (response: any) => {
        console.log('Response từ API:', response);
        if (response.status && !response.token) {
          this.errorMessage = response.mess;
          this.isLoading = false;
          return;
        }
        if (response.token && response.refreshToken) {
          console.log('Đăng nhập thành công:', response);
          const success = this.authService.setAuthTokens(
            response.token,
            response.refreshToken
          );

          if (success) {
            if (this.rememberMe) {
              localStorage.setItem('userEmail', this.loginData.email);
            }
            this.router.navigateByUrl('home');
          } else {
            this.errorMessage = 'Có lỗi xảy ra khi lưu thông tin đăng nhập';
          }
        } else {
          this.errorMessage = 'Lỗi xác thực. Vui lòng thử lại!';
        }
      },
      error: (error) => {
        console.error('Lỗi đăng nhập:', error);
        if (error.error && error.error.mess) {
          this.errorMessage = error.error.mess;
        } else {
          this.errorMessage = 'Đăng nhập thất bại. Vui lòng thử lại!';
        }
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }
}
