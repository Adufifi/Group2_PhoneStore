import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Color } from '../Interface/Color';
import { api_url } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class ColorService {
  private apiUrl = `${api_url}/ProductColor`;

  constructor(private http: HttpClient) { }

  getAllColors(): Observable<Color[]> {
    return this.http.get<Color[]>(`${this.apiUrl}/All`);
  }
}
