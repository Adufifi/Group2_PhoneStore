import { Component, Input, OnInit } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import {
  ProductItemComponent,
  Product,
} from '../product-item/product-item.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [NgFor, NgIf, ProductItemComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss',
})
export class ProductListComponent implements OnInit {
  @Input() products: Product[] = [];
  @Input() showViewMoreButton: boolean = false;
  @Input() limit: number = 6;

  ngOnInit(): void {
    if (this.products.length === 0) {
      // Mock data nếu không có dữ liệu được truyền vào
      this.products = [
        {
          id: 1,
          name: 'iPhone 14 Pro Max 128GB',
          price: 26990000,
          oldPrice: 31990000,
          image:
            'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-tim-thumb-600x600.jpg',
          discount: 15,
        },
        {
          id: 2,
          name: 'Samsung Galaxy S23 Ultra 256GB',
          price: 23990000,
          oldPrice: 26990000,
          image:
            'https://cdn.tgdd.vn/Products/Images/42/247508/Samsung-Galaxy-S22-Ultra-Burgundy-600x600.jpg',
          discount: 10,
        },
        {
          id: 3,
          name: 'OPPO Reno8 T 5G',
          price: 10990000,
          image:
            'https://cdn.tgdd.vn/Products/Images/42/301642/oppo-reno8-t-5g-256gb-cam-thumb-600x600.jpg',
        },
        {
          id: 4,
          name: 'Vivo V25 5G',
          price: 8490000,
          oldPrice: 10490000,
          image:
            'https://cdn.tgdd.vn/Products/Images/42/282828/vivo-v25-5g-xanh-thumb-1-600x600.jpg',
          discount: 20,
        },
        {
          id: 5,
          name: 'Xiaomi 12T 5G 256GB',
          price: 11990000,
          image:
            'https://cdn.tgdd.vn/Products/Images/42/283148/xiaomi-12t-den-thumb-600x600.jpg',
        },
        {
          id: 6,
          name: 'Realme C35',
          price: 3790000,
          oldPrice: 3990000,
          image:
            'https://cdn.tgdd.vn/Products/Images/42/249720/realme-c35-thumb-new-600x600.jpg',
          discount: 5,
        },
      ];
    }
  }

  loadMore(): void {
    this.limit += 6;
    // Trong trường hợp thực tế, bạn sẽ gọi API hoặc service để lấy thêm sản phẩm
    console.log('Loading more products...');
  }
}
