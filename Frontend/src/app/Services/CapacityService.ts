import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Capacity } from '../Interface/Capacity';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class CapacityService {
  private apiUrl = `${api_url}/capacity`;

  constructor(private http: HttpClient) { }

  getAllCapacities(): Observable<Capacity[]> {
    return this.http.get<Capacity[]>(`${this.apiUrl}/All`);
  }
}
