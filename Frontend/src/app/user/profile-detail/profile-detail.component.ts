import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface AccountResponse {
  id: string;
  username: string;
  email: string;
  fullName: string;
  phoneNumber: string;
  address: string;
  avatar?: string;
}

@Component({
  selector: 'app-profile-detail',
  templateUrl: './profile-detail.component.html',
  styleUrls: ['./profile-detail.component.scss'],
  standalone: true,
  imports: [CommonModule, HttpClientModule]
})
export class ProfileDetailComponent implements OnInit {
  user: AccountResponse | null = null;
  loading: boolean = false;
  error: string | null = null;

  private apiUrl = 'https://localhost:7107/api'; // Thay đổi URL này theo cấu hình API của bạn

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.getUserById(userId);
    }
  }

  getUserById(id: string): void {
    this.loading = true;
    this.error = null;
    
    this.http.get<AccountResponse>(`${this.apiUrl}/User/GetById/${id}`).subscribe({
      next: (response) => {
        this.user = response;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Không thể tải thông tin người dùng';
        this.loading = false;
        console.error('Error fetching user:', err);
      }
    });
  }
}
