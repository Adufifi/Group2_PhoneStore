import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../Services/user.service';
import { User } from '../../Interface/user';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-profile-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile-detail.component.html',
  styleUrl: './profile-detail.component.scss',
})
export class ProfileDetailComponent implements OnInit {
  user: User | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private cookieService: CookieService
  ) {}

  ngOnInit() {
    this.loadUserData();
  }

  private loadUserData() {
    const userId = this.route.snapshot.paramMap.get('id');
    console.log('User ID from URL:', userId);

    if (!userId) {
      this.error = 'Không tìm thấy ID người dùng';
      this.isLoading = false;
      return;
    }

    const token = this.cookieService.get('Authentication');
    if (!token) {
      this.error = 'Vui lòng đăng nhập để xem thông tin';
      this.isLoading = false;
      return;
    }

    this.userService.getUserById(userId).subscribe({
      next: (data) => {
        console.log('User data received:', data);
        this.user = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Lỗi khi lấy thông tin người dùng:', error);
        this.error =
          'Không thể lấy thông tin người dùng. Vui lòng thử lại sau.';
        this.isLoading = false;
      },
    });
  }
}
