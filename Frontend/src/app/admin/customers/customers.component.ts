import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';


import { HttpClientModule } from '@angular/common/http';
import { Customer } from '../../Interface/customer.interface';
import { Role, RoleService } from '../../Services/role.service';
import { CustomerService } from '../../Services/customer.service';


@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, HttpClientModule],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
})
export class CustomersComponent implements OnInit {
  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];
  roles: Role[] = [];
  searchTerm = '';
  sortBy = 'username';
  sortOrder = 'asc';
  currentPage = 1;
  pageSize = 10;
  totalCustomers = 0;
  totalPages = 1;
  pages: number[] = [];
  Math = Math;

  constructor(
    private customerService: CustomerService,
    private roleService: RoleService
  ) {}

  ngOnInit() {
    this.loadRoles();
    this.loadCustomers();
  }

  loadRoles() {
    this.roleService.getAllRoles().subscribe({
      next: (data) => {
        this.roles = data;
      },
      error: (error) => {
        console.error('Lỗi khi tải danh sách vai trò:', error);
      }
    });
  }

  getRoleName(roleId: number): string {
    const role = this.roles.find(r => r.id === roleId);
    return role ? role.roleName : 'Không xác định';
  }

  loadCustomers() {
    this.customerService.getAllCustomers().subscribe({
      next: (data) => {
        console.log('Dữ liệu nhận được:', data);
        this.customers = data.map(customer => ({
          id: customer.id,
          username: customer.username,
          email: customer.email,
          password: customer.password,
          role_id: customer.role_id,
          created_date: customer.created_date,
          updated_date: customer.updated_date,
          status: customer.status
        }));
        this.filteredCustomers = [...this.customers];
        this.totalCustomers = this.filteredCustomers.length;
        this.calculatePages();
        this.sortCustomers();
      },
      error: (error) => {
        console.error('Lỗi khi tải danh sách khách hàng:', error);
      }
    });
  }

  calculatePages() {
    this.totalPages = Math.ceil(this.totalCustomers / this.pageSize);
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
    this.filteredCustomers = this.customers.slice(start, end);
  }

  search() {
    if (!this.searchTerm.trim()) {
      this.filteredCustomers = [...this.customers];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredCustomers = this.customers.filter(
        (customer) =>
          customer.username.toLowerCase().includes(term) ||
          customer.email.toLowerCase().includes(term)
      );
    }
    this.totalCustomers = this.filteredCustomers.length;
    this.calculatePages();
    this.currentPage = 1;
    this.sortCustomers();
  }

  sortCustomers() {
    this.filteredCustomers.sort((a, b) => {
      let comparison = 0;
      if (this.sortBy === 'username') {
        comparison = a.username.localeCompare(b.username);
      } else if (this.sortBy === 'created_date') {
        comparison = new Date(a.created_date).getTime() - new Date(b.created_date).getTime();
      }
      return this.sortOrder === 'asc' ? comparison : -comparison;
    });
    this.applyPagination();
  }

  deleteCustomer(id: number) {
    this.customerService.deleteCustomer(id.toString()).subscribe({
      next: () => {
        this.customers = this.customers.filter(customer => customer.id !== id);
        this.search();
      },
      error: (error) => {
        console.error('Lỗi khi xóa khách hàng:', error);
      }
    });
  }

  openAddCustomerModal() {
    // TODO: Implement modal logic
    console.log('Opening add customer modal');
  }
}