import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserService } from '../../Services/user.service';
import { CartService } from '../../Services/CartService';
import { Cart } from '../../Interface/Cart';
import { CartDto } from '../../Interface/CartDto';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent implements OnInit {
  proceedToCheckout() {
    this.router.navigate(['/checkout']);
  }
  userServices = inject(UserService);
  router = inject(Router);
  cartServices = inject(CartService);
  listCartDto: CartDto[] = [];
  totalQuantity: number = 0;
  totalPrice: number = 0;
  ngOnInit(): void {
    this.loadCart();
  }
  loadCart() {
    const accountId = this.userServices.getAccountId();
    if (accountId == '') {
      this.router.navigate(['/login']);
    }
    this.cartServices.GetCartByAccountId(accountId).subscribe({
      next: (data) => {
        this.listCartDto = data;
        this.totalQuantity = this.listCartDto.reduce(
          (sum, item) => sum + item.quantity,
          0
        );
        this.totalPrice = this.listCartDto.reduce(
          (sum, item) => sum + item.price * item.quantity,
          0
        );
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
  removeItem(arg0: string) {
    this.cartServices.deleteCart(arg0).subscribe({
      next: (data) => {
        if (data.status === 1) {
          this.loadCart();
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
  removeAll() {
    const accountId = this.userServices.getAccountId();
    if (accountId == '') {
      this.router.navigate(['/login']);
    }
    this.cartServices.deleteAllCartByAccountId(accountId).subscribe({
      next: (data) => {
        if (data.status === 1) {
          this.loadCart();
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}
