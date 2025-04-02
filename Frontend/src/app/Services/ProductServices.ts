import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../Interface/Product';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = `${api_url}/product`;

  constructor(private http: HttpClient) {}

  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/All`);
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/GetProductById/${id}`);
  }

  createProduct(product: Omit<Product, 'id'>): Observable<any> {
    return this.http.post(`${this.apiUrl}/CreateProduct`, product);
  }

  updateProduct(id: string, product: Partial<Product>): Observable<any> {
    return this.http.put(`${this.apiUrl}/UpdateById/${id}`, product);
  }

  deleteProduct(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteById/${id}`);
  }
}
