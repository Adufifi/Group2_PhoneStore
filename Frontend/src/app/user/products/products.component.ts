import { Product } from '../../Interface/Product';
import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../Services/ProductServices';
import { BrandService } from '../../Services/BrandService';
import { Brand } from '../../Interface/Brand';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent implements OnInit {
  allProduct: Product[] = [];
  allBrands: Brand[] = [];
  productServices = inject(ProductService);
  brandServices = inject(BrandService);
  http = inject(HttpClient);
  router = inject(Router);

  ngOnInit(): void {
    this.loadBrands();
    this.loadProducts();
  }

  navigateToProduct(productId: string): void {
    this.router.navigateByUrl(`/product-detail/${productId}`);
  }

  loadProducts() {
    debugger;
    this.productServices.getAllProducts().subscribe({
      next: (data) => {
        this.allProduct = data;
      },
      error: (error: any) => {
        console.error('Error loading products:', error);
      },
    });
  }

  loadBrands(): void {
    this.brandServices.getAllBrands().subscribe({
      next: (data) => {
        this.allBrands = data;
      },
      error: (error: any) => {
        console.error('Error loading brands:', error);
      },
    });
  }
}
