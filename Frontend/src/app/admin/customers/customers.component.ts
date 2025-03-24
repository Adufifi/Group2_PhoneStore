import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
})
export class CustomersComponent {
  customers = [
    {
      id: 1,
      name: 'Nguyễn Văn An',
      email: 'nguyenvanan@example.com',
      phone: '0987654321',
      address: 'Hà Nội',
      orders: 5,
      totalSpent: 12500000,
    },
    {
      id: 2,
      name: 'Trần Thị Bình',
      email: 'tranthibinh@example.com',
      phone: '0912345678',
      address: 'Hồ Chí Minh',
      orders: 3,
      totalSpent: 8750000,
    },
    {
      id: 3,
      name: 'Lê Văn Cường',
      email: 'levancuong@example.com',
      phone: '0923456789',
      address: 'Đà Nẵng',
      orders: 2,
      totalSpent: 5200000,
    },
    {
      id: 4,
      name: 'Phạm Thị Dung',
      email: 'phamthidung@example.com',
      phone: '0934567890',
      address: 'Cần Thơ',
      orders: 1,
      totalSpent: 3100000,
    },
    {
      id: 5,
      name: 'Hoàng Văn Em',
      email: 'hoangvanem@example.com',
      phone: '0945678901',
      address: 'Hải Phòng',
      orders: 4,
      totalSpent: 9800000,
    },
  ];

  filteredCustomers = [...this.customers];
  searchTerm = '';
  sortBy = 'name';
  sortOrder = 'asc';

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
    this.customers = this.customers.filter((customer) => customer.id !== id);
    this.search();
  }
}
