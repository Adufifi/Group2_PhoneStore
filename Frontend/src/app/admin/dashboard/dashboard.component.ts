import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  statistics = [
    { title: 'Tổng sản phẩm', value: 126, icon: 'bi-box', color: 'primary' },
    { title: 'Đơn hàng mới', value: 18, icon: 'bi-cart', color: 'success' },
    {
      title: 'Doanh thu',
      value: '42.5M',
      icon: 'bi-currency-dollar',
      color: 'info',
    },
    { title: 'Khách hàng', value: 214, icon: 'bi-people', color: 'warning' },
  ];

  recentOrders = [
    {
      id: '#ORD-001',
      customer: 'Nguyễn Văn A',
      date: '15/03/2024',
      total: '2.500.000đ',
      status: 'Hoàn thành',
    },
    {
      id: '#ORD-002',
      customer: 'Trần Thị B',
      date: '14/03/2024',
      total: '1.800.000đ',
      status: 'Đang giao',
    },
    {
      id: '#ORD-003',
      customer: 'Lê Văn C',
      date: '13/03/2024',
      total: '3.200.000đ',
      status: 'Đang xử lý',
    },
    {
      id: '#ORD-004',
      customer: 'Phạm Thị D',
      date: '12/03/2024',
      total: '960.000đ',
      status: 'Hoàn thành',
    },
    {
      id: '#ORD-005',
      customer: 'Hoàng Văn E',
      date: '11/03/2024',
      total: '4.500.000đ',
      status: 'Đã hủy',
    },
  ];
}
