import { HttpClient } from '@angular/common/http';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UserService } from '../../Services/user.service';
import { Observable } from 'rxjs';
import { CheckRequest } from '../../Interface/CheckRequest';
import { jwtDecode } from 'jwt-decode';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root',
})
export class UserGuard implements CanActivate {
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
      return new Observable<boolean>((observer) => {
        observer.next(false);
        observer.complete();
      });
    }
    const decodedToken: any = jwtDecode(token);
    const userId =
      decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
    const checkRequest: CheckRequest = {
      userId: userId,
    };

    return new Observable<boolean>((observer) => {
      this.userServices.checkUser(checkRequest).subscribe({
        next: (res) => {
          if (res.status === 200) {
            observer.next(true);
          } else {
            this.router.navigate(['/login']);
            this.handler();
            observer.next(false);
          }
          observer.complete();
        },
        error: (err) => {
          this.router.navigate(['/login']);
          this.handler();
          observer.next(false);
          observer.complete();
        },
      });
    });
  }
  handler(): void {
    localStorage.clear();
    this.cookieServices.delete('Authentication', '/');
  }
}
