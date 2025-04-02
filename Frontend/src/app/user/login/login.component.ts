import { LoginVm } from './../../Interface/LoginVm';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserService } from '../../Services/user.service';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginWithGoogle() {
    throw new Error('Method not implemented.');
  }
  user: LoginVm = {
    email: '',
    password: '',
  };
  userServices = inject(UserService);
  cookieServices = inject(CookieService);
  router = inject(Router);
  authService = inject(AuthService);
  errorMessage = '';

  login() {
    // debugger;
    this.userServices.login(this.user).subscribe(
      (res) => {
        if (res.status === -9999) {
          this.errorMessage = res.mess ?? '';
        }
        if (res.status === -999) {
          this.errorMessage = res.mess ?? '';
        }
        if (res.status === -2) {
          this.errorMessage = res.mess ?? '';
        }
        if (res.status === 2) {
          this.errorMessage = res.mess ?? '';
        }
        if (res.status === 3) {
          this.cookieServices.set(
            'Authentication',
            `${res.token}`,
            undefined,
            '/',
            undefined,
            true,
            'Strict'
          );
          this.authService.login(this.user.email);
          this.router.navigateByUrl('/admin');
        }
        if (res.status === 1) {
          this.cookieServices.set(
            'Authentication',
            `${res.token}`,
            undefined,
            '/',
            undefined,
            true,
            'Strict'
          );
          this.authService.login(this.user.email);
          this.router.navigateByUrl('/home');
        }
      },
      (err) => {
        console.log(err);
        this.errorMessage = 'Server Error';
      }
    );
  }
}
