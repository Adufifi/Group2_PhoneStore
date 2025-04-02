import { Component, inject } from '@angular/core';
import { ProductService } from '../../Services/ProductServices';

@Component({
  selector: 'app-promotions',
  standalone: true,
  imports: [],
  templateUrl: './promotions.component.html',
  styleUrl: './promotions.component.scss',
})
export class PromotionsComponent {
  productServices = inject(ProductService);
}
