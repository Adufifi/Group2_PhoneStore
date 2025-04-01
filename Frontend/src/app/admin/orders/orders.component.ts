import { Component, OnInit } from '@angular/core';
import { Order } from './order.interface';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [
    {
      id: 'ORD001',
      customerName: 'Nguyễn Văn A',
      orderDate: '2024-03-15',
      status: 'pending',
      total: 15000000,
      phone: '0901234567',
      address: '123 Nguyễn Huệ, Q1, TP.HCM',
    },
    {
      id: 'ORD002',
      customerName: 'Trần Thị B',
      orderDate: '2024-03-14',
      status: 'processing',
      total: 8500000,
      phone: '0901234568',
      address: '456 Lê Lợi, Q1, TP.HCM',
    },
    // Thêm nhiều dữ liệu mẫu khác...
  ];

  // Phân trang
  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalItems: number = 0;

  // Lọc và tìm kiếm
  searchTerm: string = '';
  filterStatus: string = 'all';

  ngOnInit() {
    this.totalItems = this.orders.length;
  }

  get paginatedOrders() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredOrders.slice(startIndex, endIndex);
  }

  get filteredOrders() {
    return this.orders.filter((order) => {
      const matchesSearch =
        order.customerName
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase()) ||
        order.id.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesStatus =
        this.filterStatus === 'all' || order.status === this.filterStatus;
      return matchesSearch && matchesStatus;
    });
  }

  get totalPages() {
    return Math.ceil(this.filteredOrders.length / this.itemsPerPage);
  }

  onPageChange(page: number) {
    this.currentPage = page;
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'pending':
        return 'bg-warning';
      case 'processing':
        return 'bg-info';
      case 'completed':
        return 'bg-success';
      case 'cancelled':
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'pending':
        return 'Chờ xử lý';
      case 'processing':
        return 'Đang xử lý';
      case 'completed':
        return 'Hoàn thành';
      case 'cancelled':
        return 'Đã hủy';
      default:
        return status;
    }
  }
}
