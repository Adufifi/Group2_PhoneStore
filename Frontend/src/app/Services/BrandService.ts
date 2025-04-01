import { Injectable } from '@angular/core';
import { api_url } from '../app.config';
import { HttpClient } from '@angular/common/http';
import { Brand } from '../Interface/Brand';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BrandService {
  private apiUrl = `${api_url}/brand`;

  constructor(private http: HttpClient) {}

  getAllBrands(): Observable<Brand[]> {
    return this.http.get<Brand[]>(`${this.apiUrl}/AllBrand`);
  }

  getBrandById(id: string): Observable<Brand> {
    return this.http.get<Brand>(`${this.apiUrl}/GetBrandById/${id}`);
  }
}
