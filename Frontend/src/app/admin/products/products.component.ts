import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Product } from '../../Interface/Product';
import { ProductService } from '../../Services/ProductServices';
import { BrandService } from '../../Services/BrandService';
import { Brand } from '../../Interface/Brand';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent implements OnInit {
  allProducts: Product[] = [];
  allBrands: Brand[] = [];
  products: Product[] = [];
  searchQuery: string = '';
  sortField: string = 'Name';
  sortOrder: string = 'Ascending';

  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 0;
  paginatedProducts: Product[] = [];

  brandService = inject(BrandService);
  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
    this.loadBrands();
  }

  loadProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.allProducts = data.map((product) => ({
          ...product,
          image: product.image,
        }));
        this.applyFilters();
      },
      error: (error) => {
        console.error('Error loading products:', error);
      },
    });
  }

  applyFilters(): void {
    let filteredProducts = [...this.allProducts];

    if (this.searchQuery) {
      filteredProducts = filteredProducts.filter(
        (product) =>
          product.productName
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase()) ||
          product.brandName
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase())
      );
    }

    filteredProducts.sort((a, b) => {
      let comparison = 0;
      switch (this.sortField) {
        case 'Name':
          comparison = a.productName.localeCompare(b.productName);
          break;
        case 'Brand':
          comparison = a.brandName.localeCompare(b.brandName);
          break;
        case 'Status':
          comparison =
            a.isPromoted === b.isPromoted ? 0 : a.isPromoted ? -1 : 1;
          break;
      }
      return this.sortOrder === 'Ascending' ? comparison : -comparison;
    });

    this.products = filteredProducts;
    this.totalPages = Math.ceil(this.products.length / this.itemsPerPage);
    this.updatePaginatedProducts();
  }

  updatePaginatedProducts(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedProducts();
    }
  }

  onSearch(query: string): void {
    this.searchQuery = query;
    this.currentPage = 1;
    this.applyFilters();
  }

  onSortFieldChange(field: string): void {
    this.sortField = field;
    this.currentPage = 1;
    this.applyFilters();
  }

  onSortOrderChange(order: string): void {
    this.sortOrder = order;
    this.currentPage = 1;
    this.applyFilters();
  }

  loadBrands(): void {
    this.brandService.getAllBrands().subscribe({
      next: (brands) => {
        this.allBrands = brands;
      },
      error: (error) => {
        console.error('Error loading brands:', error);
      },
    });
  }

  deleteProduct(id: string): void {
    if (confirm('Are you sure you want to delete this product?')) {
      debugger;
      this.productService.deleteProduct(id).subscribe({
        next: (res) => {
          if (res.status === 1) {
            alert(res.mess);
            this.loadProducts();
          }
        },
        error: (error) => {
          console.error('Error deleting product:', error);
        },
      });
    }
  }

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none';
    const placeholder = img.parentElement?.querySelector(
      '.product-image-placeholder'
    );
    if (placeholder) {
      placeholder.classList.add('show');
    }
  }
}
