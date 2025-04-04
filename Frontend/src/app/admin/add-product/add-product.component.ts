import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Brand } from '../../Interface/Brand';
import { Product } from '../../Interface/Product';
import { ProductService } from '../../Services/ProductServices';
import { BrandService } from '../../Services/BrandService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class AddProductComponent implements OnInit {
  brands: Brand[] = [];
  imagePreview: string | null = null;
  errorMessage: string = '';
  newProduct: Omit<Product, 'id'> = {
    productName: '',
    brandId: '',
    brandName: '',
    price: 0,
    image: '',
    description: '',
    isPromoted: false,
    buyCount: 0,
  };

  constructor(
    private productService: ProductService,
    private brandService: BrandService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadBrands();
  }

  loadBrands(): void {
    this.brandService.getAllBrands().subscribe({
      next: (data: Brand[]) => {
        this.brands = data;
      },
      error: (error: any) => {
        console.error('Error loading brands:', error);
      },
    });
  }

  onBrandSelect(brandId: string): void {
    const selectedBrand = this.brands.find((brand) => brand.id === brandId);
    if (selectedBrand) {
      this.newProduct.brandId = brandId;
      this.newProduct.brandName = selectedBrand.name;
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onload = () => {
        this.imagePreview = reader.result as string;
        const base64String = (reader.result as string).split(',')[1];
        this.newProduct.image = base64String;
      };

      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    this.errorMessage = '';
    console.log('Sending product data:', this.newProduct);

    this.productService.createProduct(this.newProduct).subscribe({
      next: () => {
        this.router.navigate(['/admin/products']);
      },
      error: (error: any) => {
        console.error('Error creating product:', error);
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['/admin/products']);
  }
}
