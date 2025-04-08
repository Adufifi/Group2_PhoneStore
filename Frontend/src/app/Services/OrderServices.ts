import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { api_url } from '../app.config';
import { OrderDto } from '../Interface/OrderDto';
import { Observable } from 'rxjs';
import { StatusResponse } from '../Interface/StatusResponse';
import { OrderResponse } from '../Interface/OrderResponse';

@Injectable({
  providedIn: 'root',
})
export class OrderServices {
  private apiUrl = `${api_url}/order`;

  constructor(private http: HttpClient) {}

  createOrder(order: OrderDto): Observable<StatusResponse> {
    return this.http.post<StatusResponse>(`${this.apiUrl}/createOrder`, order);
  }
  getOrdersByUserId(accountId: string): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(
      `${this.apiUrl}/getAllOrderByAccountId/${accountId}`
    );
  }

  getAllOrders(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/All`);
  }

  updateOrderStatus(
    orderId: string,
    status: number
  ): Observable<StatusResponse> {
    return this.http.put<StatusResponse>(
      `${this.apiUrl}/updateOrderStatus/${orderId}`,
      { status }
    );
  }

  getOrderDetails(orderId: string): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(
      `${this.apiUrl}/getOrderDetails/${orderId}`
    );
  }
}
