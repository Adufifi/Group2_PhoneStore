import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ForgotPasswordService } from '../../Services/forgot-password.service';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, FormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent implements OnInit {
  email: string = '';
  otp: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  isOtpSent: boolean = false;
  isOtpVerified: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  timeLeft: number = 180; // 2 minutes in seconds
  timer: any;
  isLoading: boolean = false;

  constructor(
    private router: Router,
    private forgotPasswordService: ForgotPasswordService // Inject dịch vụ
  ) {}
  ngOnInit(): void {
    const storedEmail = localStorage.getItem('resetPasswordEmail');
    if (storedEmail) {
      // Nếu có email trong localStorage, thì gán email vào biến và bắt đầu timer
      this.email = storedEmail;
      this.isOtpSent = false;
      this.startTimer();
    } else {
      // Nếu không có email trong localStorage, quay lại bước nhập email
      this.isOtpSent = false;
      this.isOtpVerified = false;
      this.email = '';
    }
  }

  startTimer() {
    this.timeLeft = 180;
    this.timer = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        clearInterval(this.timer);
        this.isOtpSent = false;
        this.isOtpVerified = false;
        this.errorMessage = 'Mã OTP đã hết hạn. Vui lòng yêu cầu mã mới.';
      }
    }, 1000);
  }

  sendOtp() {
    console.log('sendOtp method was called');

    // Kiểm tra email hợp lệ
    if (!this.email || !this.validateEmail(this.email)) {
      this.errorMessage = 'Vui lòng nhập email hợp lệ';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    // Gửi yêu cầu OTP qua dịch vụ
    this.forgotPasswordService.sendOtp(this.email).subscribe({
      next: (response: string) => {
        this.isLoading = false;

        // Xử lý phản hồi từ API
        console.log('Response from API:', response);
        this.isOtpSent = true;
        localStorage.setItem('resetPasswordEmail', this.email);
        this.startTimer();
        this.successMessage =
          response || 'Mã OTP đã được gửi đến email của bạn';
        console.log('isOtpSent after success:', this.isOtpSent); // In trạng thái sau khi gửi OTP thành công
      },
      error: (error) => {
        this.isLoading = false;
        console.log('Error in sending OTP:', error); // In chi tiết lỗi

        // Xử lý lỗi từ API
        if (error.status === 404) {
          this.errorMessage = 'Email chưa được đăng ký trong hệ thống'; // Lỗi khi email không tồn tại
        } else {
          this.errorMessage = 'Có lỗi xảy ra. Vui lòng thử lại sau.';
        }

        debugger;
      },
    });
  }

  resendOtp() {
    this.isOtpSent = false;
    this.sendOtp();
  }

  verifyOtp(): void {
    // Kiểm tra định dạng OTP (6 chữ số)
    if (this.otp.length !== 6 || !/^\d+$/.test(this.otp)) {
      this.errorMessage = 'Vui lòng nhập mã OTP hợp lệ';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    // Tạo đối tượng dữ liệu
    const requestData = {
      email: this.email,
      otp: this.otp,
    };

    // Gửi yêu cầu xác thực OTP qua dịch vụ
    this.forgotPasswordService.verifyOtp(this.email, this.otp).subscribe({
      next: (response: any) => {
        this.isLoading = false;

        // Kiểm tra phản hồi từ API
        if (response.success) {
          this.isOtpVerified = true;
          clearInterval(this.timer); // Dừng bộ đếm thời gian OTP
          this.successMessage = 'Xác thực OTP thành công';
        } else {
          this.errorMessage = response.message || 'Mã OTP không chính xác';
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.log('Error in OTP verification:', error); // In chi tiết lỗi

        // Xử lý lỗi từ API
        if (error.status === 400) {
          this.errorMessage = 'Mã OTP không hợp lệ hoặc đã hết hạn';
        } else {
          this.errorMessage = 'Có lỗi xảy ra. Vui lòng thử lại sau.';
        }
      },
    });
  }

  resetPassword() {
    // Kiểm tra mật khẩu và xác nhận mật khẩu có khớp không
    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Mật khẩu xác nhận không khớp';
      return;
    }

    // Kiểm tra mật khẩu có ít nhất 8 ký tự không
    if (this.newPassword.length < 8) {
      this.errorMessage = 'Mật khẩu phải có ít nhất 8 ký tự';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    // Gửi yêu cầu đặt lại mật khẩu qua dịch vụ
    this.forgotPasswordService
      .resetPassword(this.email, this.newPassword)
      .subscribe({
        next: (response: any) => {
          this.isLoading = false;
          // Kiểm tra nếu phản hồi từ API cho biết thành công
          if (response.success) {
            this.successMessage = 'Đặt lại mật khẩu thành công!';
            localStorage.removeItem('resetPasswordEmail'); // Xóa email đã lưu trong localStorage
            setTimeout(() => {
              this.router.navigate(['/login']); // Điều hướng đến trang đăng nhập sau 2 giây
            }, 2000);
          } else {
            this.errorMessage = response.message || 'Có lỗi xảy ra.';
          }
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Lỗi khi gọi API:', error); // In chi tiết lỗi
          this.errorMessage = 'Có lỗi xảy ra. Vui lòng thử lại sau.';
        },
      });
  }

  private validateEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  formatTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
  }

  ngOnDestroy() {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }
}
