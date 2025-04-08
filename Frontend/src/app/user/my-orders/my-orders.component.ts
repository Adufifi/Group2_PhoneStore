import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderResponse } from '../../Interface/OrderResponse';
import { UserService } from '../../Services/user.service';
import { OrderServices } from '../../Services/OrderServices';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class MyOrdersComponent implements OnInit {
  userServices = inject(UserService);
  orderServices = inject(OrderServices);
  orderResponse: OrderResponse[] = [];
  router = inject(Router);
  loadOrderResponse(): void {
    let accountId = this.userServices.getAccountId();
    this.orderServices.getOrdersByUserId(accountId).subscribe(
      (response) => {
        this.orderResponse = response;
      },
      (error) => {
        console.error('Error loading order response:', error);
      }
    );
  }

  constructor() {}

  ngOnInit(): void {
    this.loadOrderResponse();
  }
}
