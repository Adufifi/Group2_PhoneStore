import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProductService } from '../../Services/ProductServices';
import { Product } from '../../Interface/Product';
import { ProductVariant } from '../../Interface/ProductVariant';
import { ProductVariantsService } from '../../Services/ProductVariantsService';

@Component({
  selector: 'app-product-variants',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-variants.component.html',
  styleUrl: './product-variants.component.scss',
})
export class ProductVariantsComponent implements OnInit {
  product: Product | null = null;
  variants: ProductVariant[] = [];
  paginatedVariants: ProductVariant[] = [];
  productId: string = '';

  // Pagination variables
  currentPage: number = 1;
  itemsPerPage: number = 4;
  totalPages: number = 0;

  private productVariantsService = inject(ProductVariantsService);
  private productService = inject(ProductService);

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id') || '';
    this.loadProduct();
    this.loadVariants();
  }

  loadProduct(): void {
    if (this.productId) {
      this.productService.getProductById(this.productId).subscribe({
        next: (product) => {
          this.product = product;
        },
        error: (error) => {
          console.error('Error loading product:', error);
        },
      });
    }
  }

  loadVariants(): void {
    if (this.productId) {
      this.productVariantsService
        .getVariantsByProductId(this.productId)
        .subscribe({
          next: (variants) => {
            this.variants = variants;
            this.totalPages = Math.ceil(
              this.variants.length / this.itemsPerPage
            );
            this.updatePaginatedVariants();
          },
          error: (error) => {
            console.error('Error loading variants:', error);
          },
        });
    }
  }

  updatePaginatedVariants(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedVariants = this.variants.slice(startIndex, endIndex);
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedVariants();
    }
  }

  deleteVariant(id: string): void {
    if (confirm('Are you sure you want to delete this variant?')) {
      this.productVariantsService.deleteVariant(id).subscribe({
        next: () => {
          this.loadVariants();
        },
        error: (error) => {
          console.error('Error deleting variant:', error);
        },
      });
    }
  }

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none';
    const container = img.parentElement;
    if (container) {
      const placeholder = container.querySelector('.variant-image-placeholder');
      if (placeholder) {
        placeholder.classList.add('show');
      }
    }
  }
}
