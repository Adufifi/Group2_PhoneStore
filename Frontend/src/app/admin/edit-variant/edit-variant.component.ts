import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductVariantsService } from '../../Services/ProductVariantsService';
import { ProductVariant } from '../../Interface/ProductVariant';
import { ProductService } from '../../Services/ProductServices';
import { Product } from '../../Interface/Product';
import { ColorService } from '../../Services/ColorService';
import { CapacityService } from '../../Services/CapacityService';
import { Color } from '../../Interface/Color';
import { Capacity } from '../../Interface/Capacity';

@Component({
  selector: 'app-edit-variant',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-variant.component.html',
  styleUrl: './edit-variant.component.scss',
})
export class EditVariantComponent implements OnInit {
  product: Product | null = null;
  variant: ProductVariant | null = null;
  productId: string = '';
  variantId: string = '';
  colors: Color[] = [];
  capacities: Capacity[] = [];
  errorMessage: string = '';
  imagePreview: string | null = null;

  private productVariantsService = inject(ProductVariantsService);
  private productService = inject(ProductService);
  private colorService = inject(ColorService);
  private capacityService = inject(CapacityService);
  private router = inject(Router);

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('productId') || '';
    this.variantId = this.route.snapshot.paramMap.get('variantId') || '';
    this.loadProduct();
    this.loadVariant();
    this.loadColors();
    this.loadCapacities();
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

  loadVariant(): void {
    if (this.variantId) {
      this.productVariantsService.getVariantById(this.variantId).subscribe({
        next: (variant) => {
          this.variant = variant;
          if (variant.image) {
            this.imagePreview = 'data:image/png;base64,' + variant.image;
          }
        },
        error: (error) => {
          console.error('Error loading variant:', error);
        },
      });
    }
  }

  loadColors(): void {
    this.colorService.getAllColors().subscribe({
      next: (colors) => {
        this.colors = colors;
      },
      error: (error) => {
        console.error('Error loading colors:', error);
      },
    });
  }

  loadCapacities(): void {
    this.capacityService.getAllCapacities().subscribe({
      next: (capacities) => {
        this.capacities = capacities;
      },
      error: (error) => {
        console.error('Error loading capacities:', error);
      },
    });
  }

  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imagePreview = e.target?.result as string;
        if (this.variant) {
          this.variant.image = (e.target?.result as string).split(',')[1];
        }
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    if (this.variant) {
      this.productVariantsService
        .updateVariant(this.variantId, this.variant)
        .subscribe({
          next: () => {
            this.router.navigate(['/admin/product-variants', this.productId]);
          },
          error: (error) => {
            this.errorMessage = 'Error updating variant: ' + error.message;
          },
        });
    }
  }

  onCancel(): void {
    this.router.navigate(['/admin/product-variants', this.productId]);
  }
}
