import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss',
})
export class SettingsComponent {
  generalSettings = {
    shopName: 'Mobile Shop',
    email: 'info@mobileshop.com',
    phone: '1900 1234',
    address: 'Số 123 Đường ABC, Quận XYZ, TP HCM',
    currency: 'VND',
    language: 'vi',
  };

  paymentSettings = {
    enableCOD: true,
    enableBankTransfer: true,
    enableMomo: false,
    enableZaloPay: false,
    bankName: 'Ngân hàng Techcombank',
    bankAccount: '19001234567890',
    bankAccountName: 'NGUYEN VAN A',
  };

  shippingSettings = {
    enableFreeShipping: true,
    freeShippingMinimum: 5000000,
    flatRate: 30000,
    enableLocalPickup: true,
  };

  saveSettings() {
    // Xử lý lưu cài đặt ở đây
    console.log('Saving settings...');
    alert('Đã lưu cài đặt thành công!');
  }
}
