import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';
import { UserService } from '../../Services/user.service';
import { Customer } from '../../Interface/customer.interface';
import { CookieService } from 'ngx-cookie-service';
import { AuthHttpService } from '../../auth/services/auth-http.service';
import { MatSnackBar } from '@angular/material/snack-bar';

declare var bootstrap: any;

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, ReactiveFormsModule],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
})
export class CustomersComponent implements OnInit {
  users: Customer[] = [];
  filteredUsers: Customer[] = [];
  searchTerm = '';
  sortBy = 'userName';
  sortOrder: 'asc' | 'desc' = 'asc';
  filterRole = '';
  currentPage = 1;
  pageSize = 10;
  totalUsers = 0;
  totalPages = 0;
  pages: number[] = [];
  Math = Math;
  isLoading = false;
  error: string | null = null;
  editUserForm: FormGroup;
  isUpdating = false;
  selectedUser: Customer | null = null;

  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService,
    private cookieService: CookieService,
    private authHttp: AuthHttpService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {
    this.editUserForm = this.fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      role_id: ['', [Validators.required]],
      img: ['']
    });
  }

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
        this.filterUsers();
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

  filterUsers() {
    let filtered = [...this.users];

    // Filter by search term
    if (this.searchTerm) {
      const searchLower = this.searchTerm.toLowerCase();
      filtered = filtered.filter(user => 
        user.userName.toLowerCase().includes(searchLower) ||
        user.email.toLowerCase().includes(searchLower)
      );
    }

    // Filter by role
    if (this.filterRole) {
      filtered = filtered.filter(user => user.role?.roleName === this.filterRole);
    }

    // Sort users
    filtered.sort((a, b) => {
      let valueA = a[this.sortBy as keyof Customer];
      let valueB = b[this.sortBy as keyof Customer];

      // Handle date sorting
      if (this.sortBy === 'created_date') {
        const dateA = valueA instanceof Date ? valueA.getTime() : 0;
        const dateB = valueB instanceof Date ? valueB.getTime() : 0;
        return this.sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
      }

      // Handle string comparison
      if (typeof valueA === 'string' && typeof valueB === 'string') {
        return this.sortOrder === 'asc' 
          ? valueA.localeCompare(valueB)
          : valueB.localeCompare(valueA);
      }

      return 0;
    });

    this.filteredUsers = filtered;
    this.totalUsers = filtered.length;
    this.totalPages = Math.ceil(this.totalUsers / this.pageSize);
    this.currentPage = 1;
    this.calculatePages();
  }

  toggleSortOrder() {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.filterUsers();
  }

  deleteUser(id: string) {
    if (confirm('Bạn có chắc chắn muốn xóa người dùng này?')) {
      this.userService.deleteUser(id).subscribe({
        next: (response: any) => {
          if (response.status === 1) {
            this.users = this.users.filter(user => user.id !== id);
            this.filterUsers();
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

  openEditUserModal(user: Customer) {
    this.selectedUser = user;
    this.editUserForm.patchValue({
      userName: user.userName,
      email: user.email,
      role_id: user.role?.id,
      img: user.img
    });
    const modal = new bootstrap.Modal(document.getElementById('editUserModal'));
    modal.show();
  }

  updateUser() {
    if (this.editUserForm.valid && this.selectedUser) {
      this.isUpdating = true;
      const updatedUser = {
        ...this.selectedUser,
        ...this.editUserForm.value
      };

      this.userService.updateUser(updatedUser).subscribe({
        next: () => {
          this.isUpdating = false;
          const modal = bootstrap.Modal.getInstance(document.getElementById('editUserModal'));
          modal.hide();
          this.loadUsers();
          this.snackBar.open('Cập nhật thông tin người dùng thành công', 'Đóng', {
            duration: 3000,
            horizontalPosition: 'end',
            verticalPosition: 'top'
          });
        },
        error: (error) => {
          this.isUpdating = false;
          this.snackBar.open('Có lỗi xảy ra khi cập nhật thông tin người dùng', 'Đóng', {
            duration: 3000,
            horizontalPosition: 'end',
            verticalPosition: 'top'
          });
          console.error('Error updating user:', error);
        }
      });
    }
  }
}