import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProductVariantsService } from '../../Services/ProductVariantsService';
import { ProductVariant } from '../../Interface/ProductVariant';
import { ProductService } from '../../Services/ProductServices';
import { Product } from '../../Interface/Product';

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
  productId: string = '';

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
          },
          error: (error) => {
            console.error('Error loading variants:', error);
          },
        });
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
