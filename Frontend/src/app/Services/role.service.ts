import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { api_url } from '../Configs/api';

export interface Role {
  id: string;
  roleName: string;
  createdDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  constructor(
    private http: HttpClient,
    private cookieService: CookieService
  ) { }

  getAllRoles(): Observable<Role[]> {
    const token = this.cookieService.get('Authentication');
    if (!token) {
      throw new Error('Không tìm thấy token xác thực');
    }

    const headers = {
      'Authorization': `Bearer ${token}`
    };

    return this.http.get<Role[]>(`${api_url}/role/All`, { headers });
  }
} 