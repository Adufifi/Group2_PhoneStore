import { api_url } from './../../app.config';
import { CanActivate, Router } from '@angular/router';

import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { jwtDecode } from 'jwt-decode';
import { UserService } from '../../Services/user.service';
import { CheckRequest } from '../../Interface/CheckRequest';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(
    private router: Router,
    private http: HttpClient,
    private cookieServices: CookieService,
    private userServices: UserService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    const token = this.cookieServices.get('Authentication');
    if (!token) {
      this.router.navigate(['/login']);
      return new Observable((observer) => observer.next(false));
    }
    debugger;
    const decodedToken: any = jwtDecode(token);
    const userId =
      decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
    const checkRequest: CheckRequest = {
      userId: userId,
    };

    return new Observable<boolean>((observer) => {
      this.userServices.checkAdmin(checkRequest).subscribe({
        next: (res) => {
          // Xem API trả về gì
          console.log('AdminGuard: checkAdmin next() →', res);
          if (res.status === 200) {
            observer.next(true);
          } else {
            observer.next(false);
            setTimeout(() => {
              this.router.navigate(['/login']);
            }, 0);
          }
          observer.complete();
        },
        error: (err) => {
          console.error('AdminGuard: checkAdmin error() =', err);
          observer.next(false);
          observer.complete();
        },
      });
    });
  }
}
