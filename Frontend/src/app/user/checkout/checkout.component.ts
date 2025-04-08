import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CheckoutData, City, District } from './checkout.interface';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { OrderDto } from '../../Interface/OrderDto';
import { OrderServices } from '../../Services/OrderServices';
import { UserService } from '../../Services/user.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  cities: City[] = [];
  districts: District[] = [];
  selectedCity: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private orderServices: OrderServices,
    private userService: UserService
  ) {
    this.checkoutForm = this.fb.group({
      paymentMethod: [0, Validators.required],
      shippingAddress: this.fb.group({
        city: ['', Validators.required],
        district: ['', Validators.required],
        street: ['', Validators.required],
      }),
      recipientName: ['', [Validators.required, Validators.minLength(3)]],
      phoneNumber: [
        '',
        [Validators.required, Validators.pattern('^[0-9]{10}$')],
      ],
    });
  }

  ngOnInit(): void {
    this.loadCities();
  }

  loadCities(): void {
    this.cities = [
      { id: '1', name: 'Hà Nội' },
      { id: '2', name: 'Thành phố Hồ Chí Minh' },
      { id: '3', name: 'Đà Nẵng' },
    ];
  }

  onCityChange(event: Event): void {
    const cityId = (event.target as HTMLSelectElement).value;
    const districtControl = this.checkoutForm.get('shippingAddress.district');

    if (districtControl) {
      if (cityId) {
        districtControl.enable();
      } else {
        districtControl.disable();
      }
      districtControl.reset();
    }

    this.districts = [];
    if (cityId) {
      this.loadDistricts(cityId);
    }
  }

  loadDistricts(cityId: string): void {
    // Dữ liệu quận/huyện theo thành phố
    this.districts = [
      // Hà Nội
      { id: '1', name: 'Ba Đình', cityId: '1' },
      { id: '2', name: 'Hoàn Kiếm', cityId: '1' },
      { id: '3', name: 'Đống Đa', cityId: '1' },
      { id: '4', name: 'Hai Bà Trưng', cityId: '1' },
      { id: '5', name: 'Hoàng Mai', cityId: '1' },
      { id: '6', name: 'Thanh Xuân', cityId: '1' },
      { id: '7', name: 'Long Biên', cityId: '1' },
      { id: '8', name: 'Nam Từ Liêm', cityId: '1' },
      { id: '9', name: 'Bắc Từ Liêm', cityId: '1' },
      { id: '10', name: 'Tây Hồ', cityId: '1' },
      { id: '11', name: 'Cầu Giấy', cityId: '1' },
      { id: '12', name: 'Hà Đông', cityId: '1' },

      // TP.HCM
      { id: '31', name: 'Quận 1', cityId: '2' },
      { id: '32', name: 'Quận 3', cityId: '2' },
      { id: '33', name: 'Quận 4', cityId: '2' },
      { id: '34', name: 'Quận 5', cityId: '2' },
      { id: '35', name: 'Quận 6', cityId: '2' },
      { id: '36', name: 'Quận 7', cityId: '2' },
      { id: '37', name: 'Quận 8', cityId: '2' },
      { id: '38', name: 'Quận 10', cityId: '2' },
      { id: '39', name: 'Quận 11', cityId: '2' },
      { id: '40', name: 'Quận 12', cityId: '2' },

      // Đà Nẵng
      { id: '53', name: 'Hải Châu', cityId: '3' },
      { id: '54', name: 'Thanh Khê', cityId: '3' },
      { id: '55', name: 'Sơn Trà', cityId: '3' },
      { id: '56', name: 'Ngũ Hành Sơn', cityId: '3' },
      { id: '57', name: 'Liên Chiểu', cityId: '3' },
      { id: '58', name: 'Cẩm Lệ', cityId: '3' },
    ].filter((d) => d.cityId === cityId);
  }

  onSubmit(): void {
    if (this.checkoutForm.valid) {
      // Lấy giá trị từ form
      const formValue = this.checkoutForm.value;

      // Tạo đối tượng OrderDto
      const order: OrderDto = {
        accountId: this.userService.getAccountId(), // Lấy từ thông tin đăng nhập
        paymentMethod: formValue.paymentMethod,
        shippingAddress: `${
          formValue.shippingAddress.street
        }, ${this.getDistrictName(
          formValue.shippingAddress.district
        )}, ${this.getCityName(formValue.shippingAddress.city)}`,
        recipientName: formValue.recipientName,
        phoneNumber: formValue.phoneNumber,
      };

      console.log('Order Data:', order);
      this.orderServices.createOrder(order).subscribe({
        next: (response) => {
          if (response.status === 1) {
            alert('Đặt hàng thành công!');
          }
        },
        error: (error) => {
          console.error('Error creating order:', error);
        },
      });
    } else {
      console.error('Form is invalid');
    }
  }

  getCityName(cityId: string): string {
    const city = this.cities.find((c) => c.id === cityId);
    return city ? city.name : 'Unknown City';
  }

  getDistrictName(districtId: string): string {
    const district = this.districts.find((d) => d.id === districtId);
    return district ? district.name : 'Unknown District';
  }
}
