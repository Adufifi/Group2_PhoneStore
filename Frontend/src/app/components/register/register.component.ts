import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  registerData = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };

  isPasswordMatch(): boolean {
    return this.registerData.password === this.registerData.confirmPassword;
  }

  onSubmit() {
    if (
      this.registerData.username &&
      this.registerData.email &&
      this.registerData.password &&
      this.isPasswordMatch()
    ) {
      // TODO: Implement register logic here
      console.log('Register attempt with:', this.registerData);
    }
  }
}
