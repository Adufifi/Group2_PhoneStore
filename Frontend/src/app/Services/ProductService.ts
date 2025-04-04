import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../Interface/Product';
import { api_url } from '../app.config';

@Injectable({
    providedIn: 'root',
})
export class ProductService {
    private apiUrl = `${api_url}/products`;

    constructor(private http: HttpClient) { }

    getAllProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(`${this.apiUrl}`);
    }

    getProductById(id: string): Observable<Product> {
        return this.http.get<Product>(`${this.apiUrl}/${id}`);
    }

    getProductsByBrand(brandId: string): Observable<Product[]> {
        return this.http.get<Product[]>(`${this.apiUrl}/brand/${brandId}`);
    }

    getPromotedProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(`${this.apiUrl}/promoted`);
    }

    createProduct(product: Omit<Product, 'id'>): Observable<any> {
        return this.http.post(`${this.apiUrl}`, product);
    }

    updateProduct(id: string, product: Partial<Product>): Observable<any> {
        return this.http.put(`${this.apiUrl}/${id}`, product);
    }

    deleteProduct(id: string): Observable<any> {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }
} 