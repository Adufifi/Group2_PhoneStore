import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss',
})
export class ReportComponent {
  monthlySales = [
    { month: 'Tháng 1', revenue: 45000000, orders: 42 },
    { month: 'Tháng 2', revenue: 68000000, orders: 56 },
    { month: 'Tháng 3', revenue: 52000000, orders: 48 },
    { month: 'Tháng 4', revenue: 75000000, orders: 62 },
    { month: 'Tháng 5', revenue: 85000000, orders: 78 },
    { month: 'Tháng 6', revenue: 92000000, orders: 84 },
  ];

  topProducts = [
    { name: 'iPhone 15 Pro Max', sales: 35, revenue: 115500000 },
    { name: 'Samsung Galaxy S24 Ultra', sales: 28, revenue: 84000000 },
    { name: 'Xiaomi 14 Pro', sales: 22, revenue: 44000000 },
    { name: 'Google Pixel 8 Pro', sales: 15, revenue: 36000000 },
    { name: 'Oppo Find X7 Ultra', sales: 18, revenue: 45000000 },
  ];

  topCategories = [
    { name: 'Apple', sales: 85, percentage: 35 },
    { name: 'Samsung', sales: 65, percentage: 27 },
    { name: 'Xiaomi', sales: 42, percentage: 18 },
    { name: 'Oppo', sales: 28, percentage: 12 },
    { name: 'Google', sales: 18, percentage: 8 },
  ];
}
