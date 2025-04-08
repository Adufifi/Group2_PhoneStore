import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { api_url } from '../app.config';
import { Color } from '../Interface/Color';

@Injectable({
  providedIn: 'root',
})
export class ColorService {
  private apiUrl = `${api_url}/color`;

  constructor(private http: HttpClient) {}

  getAllColors(): Observable<Color[]> {
    return this.http.get<Color[]>(`${this.apiUrl}/All`);
  }
}
