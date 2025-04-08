import { Injectable } from '@angular/core';
import { api_url } from '../app.config';
import { HttpClient } from '@angular/common/http';
import { Cart } from '../Interface/Cart';
import { Observable } from 'rxjs';
import { StatusResponse } from '../Interface/StatusResponse';
import { AddCart } from '../Interface/AddCart';
import { CartDto } from '../Interface/CartDto';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private apiUrl = `${api_url}/cart`;

  constructor(private http: HttpClient) {}

  getAllCarts(): Observable<Cart[]> {
    return this.http.get<Cart[]>(`${this.apiUrl}`);
  }
  GetCartByAccountId(accountId: string): Observable<CartDto[]> {
    return this.http.get<CartDto[]>(
      `${this.apiUrl}/GetCartByAccountId/${accountId}`
    );
  }
  getCartById(id: string): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiUrl}/${id}`);
  }

  createCart(cart: Omit<Cart, 'id' | 'createdDate'>): Observable<any> {
    return this.http.post(`${this.apiUrl}`, cart);
  }

  createCartWithProductVariantId(addCart: AddCart): Observable<StatusResponse> {
    return this.http.post<StatusResponse>(`${this.apiUrl}/AddCart`, addCart);
  }

  updateCart(id: string, cart: Partial<Cart>): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, cart);
  }

  deleteCart(id: string): Observable<StatusResponse> {
    return this.http.delete<StatusResponse>(
      `${this.apiUrl}/DeleteCartById/${id}`
    );
  }
  deleteAllCartByAccountId(accountId: string): Observable<StatusResponse> {
    return this.http.delete<StatusResponse>(
      `${this.apiUrl}/DeleteAllCartByAccountId/${accountId}`
    );
  }
}
