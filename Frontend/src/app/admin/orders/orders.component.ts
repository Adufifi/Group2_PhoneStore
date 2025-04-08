import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Order } from './order.interface';
import { OrderServices } from '../../Services/OrderServices';
import { OrderResponse } from '../../Interface/OrderResponse';
import { StatusResponse } from '../../Interface/StatusResponse';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  loading: boolean = true;
  error: string | null = null;

  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalItems: number = 0;

  searchTerm: string = '';
  filterStatus: string = 'all';

  constructor(private orderService: OrderServices) {}

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;
    this.error = null;
    this.orderService.getAllOrders().subscribe({
      next: (response: OrderResponse[]) => {
        try {
          this.orders = response.map((order) => ({
            id: order.id || 'N/A',
            customerName: order.recipientName || 'N/A',
            orderDate: order.createdDate || 'N/A',
            status: this.mapStatus(order.statusProduct || 0),
            total:
              order.totalAmount ||
              this.calculateTotal(order.productVariantResponse || []),
            phone: order.phoneNumber || 'N/A',
            address: order.shippingAddress || 'N/A',
          }));
          this.totalItems = this.orders.length;
        } catch (err) {
          console.error('Error processing orders:', err);
          this.error = 'Error processing orders data. Please try again later.';
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load orders. Please try again later.';
        this.loading = false;
        console.error('Error loading orders:', err);
      },
    });
  }

  private mapStatus(
    statusProduct: number
  ): 'pending' | 'processing' | 'completed' | 'cancelled' {
    switch (statusProduct) {
      case 0:
        return 'pending';
      case 1:
        return 'processing';
      case 2:
        return 'completed';
      case 3:
        return 'cancelled';
      default:
        return 'pending';
    }
  }

  private calculateTotal(products: any[]): number {
    if (!Array.isArray(products)) {
      return 0;
    }
    return products.reduce((total, product) => {
      const price = product?.price || 0;
      const quantity = product?.quantity || 0;
      return total + price * quantity;
    }, 0);
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
