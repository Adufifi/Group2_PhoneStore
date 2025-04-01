import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../auth/services/auth.service';
import { CookieService } from 'ngx-cookie-service';

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

  private apiUrl = 'https://localhost:7107/api';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private authService: AuthService,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {
    // Lấy ID từ token
    const userId = this.authService.getUserId();
    console.log('User ID from token:', userId);

    if (userId) {
      // Lấy thông tin người dùng từ API
      this.getUserById(userId);
    } else {
      this.error = 'Không tìm thấy thông tin người dùng';
    }
  }

  getUserById(id: string): void {
    this.loading = true;
    this.error = null;
    
    console.log('Fetching user with ID:', id);
    this.http.get<AccountResponse>(`${this.apiUrl}/User/GetById/${id}`).subscribe({
      next: (response) => {
        console.log('User data received:', response);
        this.user = response;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error details:', err);
        this.error = 'Không thể tải thông tin người dùng';
        this.loading = false;
      }
    });
  }
}
