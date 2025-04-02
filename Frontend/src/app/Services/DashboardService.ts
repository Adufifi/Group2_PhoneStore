import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private http: HttpClient) {}
  getAllOrders(): Observable<any[]> {
    return this.http.get<any[]>(`${api_url}/Order/getAllOrders`);
  }
  getProductVariantById(id: string): Observable<any> {
    return this.http.get<any>(`${api_url}/ProductVariants/${id}`);
  }

  getAccountById(id: string): Observable<any> {
    return this.http.get<any>(`${api_url}/account/GetById${id}`);
  }
  updateOrder(orderId: string, updatedOrder: any): Observable<any> {
    return this.http.put(
      `${api_url}/Order/updateOrder/${orderId}`,
      updatedOrder
    );
  }

  getOrderItemsByOrderId(id: string): Observable<any> {
    return this.http.get<any>(
      `${api_url}/OrderItem/getOrderItemsByOrderId/${id}`
    );
  }
}
