import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../../Services/customer.service';
import { Customer } from '../../Models/customer.interface';
import { HttpClientModule } from '@angular/common/http';

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
  searchTerm = '';
  sortBy = 'name';
  sortOrder = 'asc';

  constructor(private customerService: CustomerService) {}

  ngOnInit() {
    this.loadCustomers();
  }

  loadCustomers() {
    this.customerService.getAllCustomers().subscribe({
      next: (data) => {
        this.customers = data;
        this.filteredCustomers = [...this.customers];
      },
      error: (error) => {
        console.error('Lỗi khi tải danh sách khách hàng:', error);
      }
    });
  }

  search() {
    if (!this.searchTerm.trim()) {
      this.filteredCustomers = [...this.customers];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredCustomers = this.customers.filter(
        (customer) =>
          customer.name.toLowerCase().includes(term) ||
          customer.email.toLowerCase().includes(term) ||
          customer.phone.includes(term)
      );
    }
    this.sortCustomers();
  }

  sortCustomers() {
    this.filteredCustomers.sort((a, b) => {
      let comparison = 0;
      if (this.sortBy === 'name') {
        comparison = a.name.localeCompare(b.name);
      } else if (this.sortBy === 'orders') {
        comparison = a.orders - b.orders;
      } else if (this.sortBy === 'totalSpent') {
        comparison = a.totalSpent - b.totalSpent;
      }
      return this.sortOrder === 'asc' ? comparison : -comparison;
    });
  }

  deleteCustomer(id: number) {
    this.customerService.deleteCustomer(id).subscribe({
      next: () => {
        this.customers = this.customers.filter(customer => customer.id !== id);
        this.search();
      },
      error: (error) => {
        console.error('Lỗi khi xóa khách hàng:', error);
      }
    });
  }
}