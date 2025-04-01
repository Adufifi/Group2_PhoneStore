import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../Services/user.service';
import { User } from '../../Interface/user';


@Component({
  selector: 'app-profile-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-detail.component.html',
  styleUrl: './profile-detail.component.scss'
})
export class ProfileDetailComponent implements OnInit {
  user: User | null = null;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService
  ) {}

  ngOnInit() {
    // Lấy ID từ URL
    const userId = this.route.snapshot.paramMap.get('id');
    console.log('User ID from URL:', userId);

    if (userId) {
      // Gọi API lấy thông tin người dùng
      this.userService.getUserById(userId).subscribe({
        next: (data: User) => {
          this.user = data;
          console.log('Thông tin người dùng:', this.user);
        },
        error: (error: any) => {
          console.error('Lỗi khi lấy thông tin người dùng:', error);
          this.error = 'Không thể lấy thông tin người dùng';
        }
      });
    } else {
      console.error('Không tìm thấy ID người dùng trong URL');
      this.error = 'Không tìm thấy ID người dùng';
    }
  }
}
