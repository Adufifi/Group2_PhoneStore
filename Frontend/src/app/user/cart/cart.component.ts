import { Component, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent implements OnInit {
  constructor(private router: Router) {}

  ngOnInit() {
    const userEmail = localStorage.getItem('email');
    if (!userEmail) {
      this.router.navigate(['/login']);
    }
  }
}
