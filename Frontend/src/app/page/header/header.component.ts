import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  standalone: true,
})
export class HeaderComponent implements OnInit {
  isLoggedIn = false;
  userEmail: string | null = null;

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit() {
    this.authService.isLoggedIn.subscribe((status: boolean) => {
      this.isLoggedIn = status;
      this.userEmail = status ? localStorage.getItem('email') : null;
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
