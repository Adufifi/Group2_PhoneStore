import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';

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

  constructor(private router: Router) {}

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
      console.log('Đăng ký với:', this.registerData);
      alert('Đăng ký thành công!');
      this.router.navigate(['/login']);
    }
  }
}
