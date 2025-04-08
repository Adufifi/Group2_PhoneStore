import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductVariant } from '../../Interface/ProductVariant';
import { ProductVariantsService } from '../../Services/ProductVariantsService';
import { ProductService } from '../../Services/ProductServices';
import { Product } from '../../Interface/Product';
import { CapacityService } from '../../Services/CapacityService';
import { ColorService } from '../../Services/ColorService';
import { Color } from '../../Interface/Color';
import { Capacity } from '../../Interface/Capacity';

@Component({
  selector: 'app-add-variant',
  templateUrl: './add-variant.component.html',
  styleUrls: ['./add-variant.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  providers: [
    ColorService,
    CapacityService,
    ProductService,
    ProductVariantsService,
  ],
})
export class AddVariantComponent implements OnInit {
  product: Product | null = null;
  productId: string = '';
  colors: Color[] = [];
  capacities: Capacity[] = [];
  imagePreview: string | null = null;
  errorMessage: string = '';

  newVariant: Omit<
    ProductVariant,
    'id' | 'productName' | 'colorName' | 'capacityName'
  > = {
    productId: '',
    colorId: '',
    capacityId: '',
    price: 0,
    quantity: 0,
    image: '',
  };

  private http = inject(HttpClient);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productVariantsService = inject(ProductVariantsService);
  private productService = inject(ProductService);
  private colorService = inject(ColorService);
  private capacityService = inject(CapacityService);

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id') || '';
    this.newVariant.productId = this.productId;
    this.loadProduct();
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

  loadColors(): void {
    this.colorService.getAllColors().subscribe({
      next: (colors: Color[]) => {
        this.colors = colors;
      },
      error: (error: Error) => {
        console.error('Error loading colors:', error);
      },
    });
  }

  loadCapacities(): void {
    this.capacityService.getAllCapacities().subscribe({
      next: (capacities: Capacity[]) => {
        this.capacities = capacities;
      },
      error: (error: Error) => {
        console.error('Error loading capacities:', error);
      },
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onload = () => {
        this.imagePreview = reader.result as string;
        const base64String = (reader.result as string).split(',')[1];
        this.newVariant.image = base64String;
      };

      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    this.errorMessage = '';
    console.log('Sending variant data:', this.newVariant);

    this.productVariantsService.createVariant(this.newVariant).subscribe({
      next: () => {
        this.router.navigate(['/admin/product-variants', this.productId]);
      },
      error: (error) => {
        console.error('Error creating variant:', error);
        this.errorMessage = 'Error creating variant. Please try again.';
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['/admin/product-variants', this.productId]);
  }
}
