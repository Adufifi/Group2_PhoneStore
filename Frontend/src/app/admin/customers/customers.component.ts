import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';
import { UserService } from '../../Services/user.service';
import { User } from '../../Interface/user';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
})
export class CustomersComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm = '';
  sortBy = 'username';
  sortOrder = 'asc';
  currentPage = 1;
  pageSize = 10;
  totalUsers = 0;
  totalPages = 1;
  pages: number[] = [];
  Math = Math;
  isLoading = false;
  error: string | null = null;

  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading = true;
    this.error = null;

    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.filteredUsers = [...this.users];
        this.totalUsers = this.filteredUsers.length;
        this.calculatePages();
        this.sortUsers();
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Không thể tải danh sách người dùng';
        this.isLoading = false;
        console.error('Lỗi khi tải danh sách người dùng:', error);
      }
    });
  }

  calculatePages() {
    this.totalPages = Math.ceil(this.totalUsers / this.pageSize);
    this.pages = Array.from({length: this.totalPages}, (_, i) => i + 1);
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.applyPagination();
    }
  }

  applyPagination() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.filteredUsers = this.users.slice(start, end);
  }

  search() {
    if (!this.searchTerm.trim()) {
      this.filteredUsers = [...this.users];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredUsers = this.users.filter(
        (user) =>
          user.userName.toLowerCase().includes(term) ||
          user.email.toLowerCase().includes(term)
      );
    }
    this.totalUsers = this.filteredUsers.length;
    this.calculatePages();
    this.currentPage = 1;
    this.sortUsers();
  }

  sortUsers() {
    this.filteredUsers.sort((a, b) => {
      let comparison = 0;
      if (this.sortBy === 'username') {
        comparison = a.userName.localeCompare(b.userName);
      } else if (this.sortBy === 'email') {
        comparison = a.email.localeCompare(b.email);
      }
      return this.sortOrder === 'asc' ? comparison : -comparison;
    });
    this.applyPagination();
  }

  deleteUser(id: string) {
    if (confirm('Bạn có chắc chắn muốn xóa người dùng này?')) {
      this.userService.deleteUser(id).subscribe({
        next: (response: any) => {
          if (response.status === 1) {
            this.users = this.users.filter(user => user.id !== id);
            this.search();
          } else {
            this.error = response.mess || 'Không thể xóa người dùng';
          }
        },
        error: (error: Error) => {
          this.error = 'Lỗi khi xóa người dùng: ' + error.message;
          console.error('Lỗi khi xóa người dùng:', error);
        }
      });
    }
  }

  openAddUserModal() {
    // TODO: Implement modal logic
    console.log('Opening add user modal');
  }
}