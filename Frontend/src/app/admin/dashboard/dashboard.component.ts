import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { api_url } from '../../app.config';
import { DashboardService } from '../../Services/DashboardService';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  statistics = [
    { title: 'Tổng sản phẩm', value: 0, icon: 'bi-box', color: 'primary' },
    { title: 'Đơn hàng mới', value: 0, icon: 'bi-cart', color: 'success' },
    {
      title: 'Doanh thu',
      value: '0',
      icon: 'bi-currency-dollar',
      color: 'info',
    },
    { title: 'Khách hàng', value: 0, icon: 'bi-people', color: 'warning' },
  ];

  recentOrders: any[] = [];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.getRecentOrders();
  }

  // Lấy tất cả đơn hàng
  getRecentOrders(): void {
    this.dashboardService.getAllOrders().subscribe(
      (data: any[]) => {
        // Sắp xếp các đơn hàng theo thứ tự từ mới đến cũ
        this.recentOrders = data.sort(
          (a, b) =>
            new Date(b.createdDate).getTime() -
            new Date(a.createdDate).getTime()
        );

        // Lặp qua các đơn hàng và lấy tên người dùng từ API account và thông tin orderItems
        const orderRequests = this.recentOrders.map(async (order) => {
          console.log('order.accountId:', order.accountId);
          if (order.accountId) {
            try {
              const userName = await this.getUserName(order.accountId);
              order.UserName = userName;
            } catch (error) {
              console.error('Lỗi khi lấy tên người dùng:', error);
              order.UserName = 'Không có tên';
            }
          } else {
            console.error('accountId is missing for order', order);
            order.UserName = 'Không có tên';
          }

          try {
            console.log(`Fetching orderItems for OrderId: ${order.id}`);
            const total = await this.getOrderItems(order.id);
            order.total = total;
            console.log(`Total for Order ${order.id}: ${total}`);
          } catch (error) {
            console.error(
              `OrderItems not found for order ID: ${order.id}`,
              error
            );
            order.total = 0;
          }

          return order;
        });

        Promise.all(orderRequests).then((updatedOrders) => {
          this.recentOrders = updatedOrders;
          this.calculateStatistics();
          console.log('Updated Orders:', this.recentOrders);
        });
      },
      (error: any) => {
        console.error('Lỗi khi lấy dữ liệu đơn hàng: ', error);
      }
    );
  }

  // Lấy tên người dùng từ accountId
  getUserName(accountId: string): Promise<string> {
    console.log('Fetching UserName for AccountId:', accountId);
    if (!accountId) {
      console.error('accountId is undefined or null');
      return Promise.resolve('Không có tên');
    }

    return this.dashboardService
      .getAccountById(accountId)
      .toPromise()
      .then(
        (data) => {
          console.log('Dữ liệu tài khoản:', data);
          return data.userName || 'Không có tên';
        },
        (error) => {
          console.error('Lỗi khi lấy thông tin tài khoản: ', error);
          return 'Không có tên';
        }
      );
  }

  // Lấy thông tin OrderItems và tính tổng tiền cho mỗi đơn hàng
  getOrderItems(orderId: string): Promise<number> {
    console.log('Fetching OrderItems for OrderId:', orderId);
    return this.dashboardService
      .getOrderItemsByOrderId(orderId)
      .toPromise()
      .then(
        async (orderItems) => {
          let total = 0;
          if (orderItems && Array.isArray(orderItems)) {
            for (const item of orderItems) {
              try {
                const productVariant = await this.dashboardService
                  .getProductVariantById(item.productVariantsId)
                  .toPromise();
                const price = productVariant.price || 0;
                total += price * item.quantity;
              } catch (error) {
                console.error(
                  `Lỗi khi lấy ProductVariant với ID ${item.productVariantsId}`,
                  error
                );
              }
            }
          }
          return total;
        },
        (error) => {
          console.error('Lỗi khi lấy thông tin OrderItems: ', error);
          return 0;
        }
      );
  }

  // Tính toán các thống kê
  calculateStatistics(): void {
    let totalOrders = 0;
    let totalRevenue = 0;
    let newOrders = 0;
    let customers: Set<any> = new Set();

    this.recentOrders.forEach((order) => {
      totalOrders++;
      totalRevenue += order.total;
      if (order.status === 2 || order.status === 1) {
        newOrders++;
      }
      customers.add(order.UserName);
    });

    this.statistics[1].value = newOrders;
    this.statistics[2].value = `${totalRevenue.toFixed(2)} VND`;
    this.statistics[3].value = customers.size;
    this.statistics[0].value = totalOrders;
  }

  // Chuyển đổi trạng thái đơn hàng từ số sang văn bản
  getStatusText(statusProduct: number): string {
    console.log('Status:', statusProduct);
    switch (statusProduct) {
      case 0:
        return 'Đã hủy';
      case 1:
        return 'Đang xử lý';
      case 2:
        return 'Đang giao';
      case 3:
        return 'Hoàn thành';
      default:
        return 'Không xác định';
    }
  }
}
