import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductVariant } from '../Interface/ProductVariant';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class ProductVariantsService {
  private apiUrl = `${api_url}/productVariants`;

  constructor(private http: HttpClient) { }

  getAllVariants(): Observable<ProductVariant[]> {
    return this.http.get<ProductVariant[]>(`${this.apiUrl}`);
  }

  getVariantById(id: string): Observable<ProductVariant> {
    return this.http.get<ProductVariant>(`${this.apiUrl}/${id}`);
  }

  getVariantsByProductId(productId: string): Observable<ProductVariant[]> {
    return this.http.get<ProductVariant[]>(
      `${this.apiUrl}/product/${productId}`
    );
  }

  getVariantsByColorId(colorId: string): Observable<ProductVariant[]> {
    return this.http.get<ProductVariant[]>(
      `${this.apiUrl}/color/${colorId}`
    );
  }

  getVariantsByCapacityId(capacityId: string): Observable<ProductVariant[]> {
    return this.http.get<ProductVariant[]>(
      `${this.apiUrl}/capacity/${capacityId}`
    );
  }

  createVariant(variant: Omit<ProductVariant, 'id'>): Observable<any> {
    return this.http.post(`${this.apiUrl}`, variant);
  }

  updateVariant(id: string, variant: Partial<ProductVariant>): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, variant);
  }

  deleteVariant(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
