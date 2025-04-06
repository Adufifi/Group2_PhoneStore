import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../Services/ProductServices';
import { BrandService } from '../../Services/BrandService';
import { Product } from '../../Interface/Product';
import { Brand } from '../../Interface/Brand';

@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.scss',
})
export class EditProductComponent implements OnInit {
  product: Product | null = null;
  brands: Brand[] = [];
  errorMessage: string = '';
  imagePreview: string | null = null;

  private productService = inject(ProductService);
  private brandService = inject(BrandService);
  private router = inject(Router);

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.loadProduct(productId);
    }
    this.loadBrands();
  }

  loadProduct(id: string): void {
    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.product = product;
        if (product.image) {
          this.imagePreview = 'data:image/png;base64,' + product.image;
        }
      },
      error: (error) => {
        this.errorMessage = 'Error loading product: ' + error.message;
      },
    });
  }

  loadBrands(): void {
    this.brandService.getAllBrands().subscribe({
      next: (brands) => {
        this.brands = brands;
      },
      error: (error) => {
        this.errorMessage = 'Error loading brands: ' + error.message;
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
        if (this.product) {
          this.product.image = (e.target?.result as string).split(',')[1];
        }
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    if (this.product) {
      this.productService
        .updateProduct(this.product.id, this.product)
        .subscribe({
          next: () => {
            this.router.navigate(['/admin/products']);
          },
          error: (error) => {
            this.errorMessage = 'Error updating product: ' + error.message;
          },
        });
    }
  }

  onCancel(): void {
    this.router.navigate(['/admin/products']);
  }
}
