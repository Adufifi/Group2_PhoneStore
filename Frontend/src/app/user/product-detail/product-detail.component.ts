import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Product } from '../../Interface/Product';
import { ProductVariant } from '../../Interface/ProductVariant';
import { Color } from '../../Interface/Color';
import { Capacity } from '../../Interface/Capacity';
import { ProductService } from '../../Services/ProductServices';
import { ProductVariantsService } from '../../Services/ProductVariantsService';
import { ColorService } from '../../Services/ColorService';
import { CapacityService } from '../../Services/CapacityService';
import { BrandService } from '../../Services/BrandService';
import { Brand } from '../../Interface/Brand';
import { ReviewService } from '../../Services/reviewService';
import { Review } from '../../Interface/Review';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: Product | null = null;
  brand: Brand | null = null;
  variants: ProductVariant[] = [];
  uniqueColors: Color[] = [];
  availableCapacities: Capacity[] = [];
  selectedVariant: ProductVariant | null = null;
  selectedColor: Color | null = null;
  selectedCapacity: Capacity | null = null;
  quantity: number = 1;
  errorMessage: string = '';
  reviews: Review[] = [];
  averageRating: number = 0;
  newReview: { rating: number; comment: string } = { rating: 5, comment: '' };
  isSubmittingReview: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private productVariantsService: ProductVariantsService,
    private colorService: ColorService,
    private capacityService: CapacityService,
    private brandService: BrandService,
    private reviewService: ReviewService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const productId = params['id'];
      if (productId) {
        this.loadProduct(productId);
        this.loadVariants(productId);
        this.loadReviews(productId);
      }
    });
  }

  loadProduct(productId: string): void {
    this.productService.getProductById(productId).subscribe({
      next: (product) => {
        this.product = product;
        this.loadBrand(product.brandId);
      },
      error: (error) => {
        this.errorMessage = 'Error loading product: ' + error.message;
      }
    });
  }

  loadBrand(brandId: string): void {
    this.brandService.getBrandById(brandId).subscribe({
      next: (brand) => {
        this.brand = brand;
      },
      error: (error) => {
        console.error('Error loading brand:', error);
      }
    });
  }

  loadVariants(productId: string): void {
    console.log('Loading variants for product ID:', productId);
    this.productVariantsService.getVariantsByProductId(productId).subscribe({
      next: (variants) => {
        console.log('Variants loaded:', variants);
        if (variants && variants.length > 0) {
          this.variants = variants;
          this.extractUniqueColorsAndCapacities();
        } else {
          console.log('No variants found for product ID:', productId);
          this.errorMessage = 'No variants available for this product';
        }
      },
      error: (error) => {
        console.error('Error loading variants:', error);
        this.errorMessage = 'Error loading variants: ' + error.message;
      }
    });
  }

  extractUniqueColorsAndCapacities(): void {
    console.log('Extracting unique colors and capacities from variants:', this.variants);

    // Extract unique colors
    const colorMap = new Map<string, Color>();
    this.variants.forEach(variant => {
      console.log('Processing variant for colors:', variant);
      if (variant.colorId && variant.colorName) {
        if (!colorMap.has(variant.colorId)) {
          colorMap.set(variant.colorId, {
            id: variant.colorId,
            colorName: variant.colorName
          });
        }
      }
    });
    this.uniqueColors = Array.from(colorMap.values());
    console.log('Unique colors extracted:', this.uniqueColors);

    // Extract unique capacities
    const capacityMap = new Map<string, Capacity>();
    this.variants.forEach(variant => {
      console.log('Processing variant for capacities:', variant);
      if (variant.capacityId && variant.capacityName) {
        if (!capacityMap.has(variant.capacityId)) {
          capacityMap.set(variant.capacityId, {
            id: variant.capacityId,
            capacityName: variant.capacityName
          });
        }
      }
    });
    this.availableCapacities = Array.from(capacityMap.values());
    console.log('Unique capacities extracted:', this.availableCapacities);

    // If we have colors and capacities, select the first one by default
    if (this.uniqueColors.length > 0) {
      this.selectedColor = this.uniqueColors[0];
    }
    if (this.availableCapacities.length > 0) {
      this.selectedCapacity = this.availableCapacities[0];
    }

    // Update the selected variant based on the default selections
    this.updateSelectedVariant();
  }

  selectColor(color: Color): void {
    this.selectedColor = color;
    this.updateSelectedVariant();
  }

  selectCapacity(capacity: Capacity): void {
    this.selectedCapacity = capacity;
    this.updateSelectedVariant();
  }

  updateSelectedVariant(): void {
    if (!this.selectedColor || !this.selectedCapacity) {
      this.selectedVariant = null;
      return;
    }

    this.selectedVariant = this.variants.find(variant =>
      variant.colorId === this.selectedColor?.id &&
      variant.capacityId === this.selectedCapacity?.id
    ) || null;
  }

  selectVariant(variant: ProductVariant): void {
    this.selectedVariant = variant;
    this.selectedColor = { id: variant.colorId, colorName: variant.colorName || '' };
    this.selectedCapacity = { id: variant.capacityId, capacityName: variant.capacityName || '' };
  }

  increaseQuantity(): void {
    if (this.selectedVariant && this.quantity < this.selectedVariant.quantity) {
      this.quantity++;
    }
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  addToCart(): void {
    if (this.selectedVariant) {
      // Implement add to cart functionality
      console.log('Adding to cart:', this.selectedVariant, 'Quantity:', this.quantity);
    }
  }

  buyNow(): void {
    if (this.selectedVariant) {
      // Implement buy now functionality
      console.log('Buying now:', this.selectedVariant, 'Quantity:', this.quantity);
    }
  }

  getImageUrl(image: string | null | undefined): string {
    if (!image) return '';
    if (image.startsWith('data:image')) return image;
    if (image.startsWith('http')) return image;
    return `data:image/png;base64,${image}`;
  }

  loadReviews(productId: string): void {
    this.reviewService.getReviewsByProductId(productId).subscribe({
      next: (reviews: Review[]) => {
        this.reviews = reviews;
        this.calculateAverageRating();
      },
      error: (error: any) => {
        console.error('Error loading reviews:', error);
      }
    });
  }

  calculateAverageRating(): void {
    if (this.reviews.length === 0) {
      this.averageRating = 0;
      return;
    }

    const totalRating = this.reviews.reduce((sum, review) => sum + review.rating, 0);
    this.averageRating = totalRating / this.reviews.length;
  }

  submitReview(): void {
    if (!this.product || !this.newReview.comment.trim()) return;

    this.isSubmittingReview = true;
    const review: Omit<Review, 'id'> = {
      productId: this.product.id,
      userId: 'user-id', // Cần thay thế bằng ID người dùng thực tế
      userName: 'User', // Cần thay thế bằng tên người dùng thực tế
      rating: this.newReview.rating,
      comment: this.newReview.comment.trim(),
      createdDate: new Date()
    };

    this.reviewService.createReview(review).subscribe({
      next: () => {
        this.loadReviews(this.product!.id);
        this.newReview = { rating: 5, comment: '' };
        this.isSubmittingReview = false;
      },
      error: (error: any) => {
        console.error('Error submitting review:', error);
        this.isSubmittingReview = false;
      }
    });
  }
} 