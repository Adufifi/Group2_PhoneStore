import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent {
  products = [
    {
      id: 1,
      name: 'iPhone 15 Pro Max',
      category: 'Apple',
      price: 32990000,
      stock: 25,
      image:
        'https://images.unsplash.com/photo-1696435026934-d16ad4665929?q=80&w=1976&auto=format&fit=crop',
    },
    {
      id: 2,
      name: 'Samsung Galaxy S24 Ultra',
      category: 'Samsung',
      price: 29990000,
      stock: 30,
      image:
        'https://images.unsplash.com/photo-1707383848513-6ac3f4b75fce?q=80&w=1974&auto=format&fit=crop',
    },
    {
      id: 3,
      name: 'Google Pixel 8 Pro',
      category: 'Google',
      price: 23990000,
      stock: 15,
      image:
        'https://images.unsplash.com/photo-1696447965426-d72d9da71f35?q=80&w=1975&auto=format&fit=crop',
    },
    {
      id: 4,
      name: 'Xiaomi 14 Pro',
      category: 'Xiaomi',
      price: 19990000,
      stock: 20,
      image:
        'https://images.unsplash.com/photo-1646562201103-00715a461fc8?q=80&w=1936&auto=format&fit=crop',
    },
    {
      id: 5,
      name: 'Oppo Find X7 Ultra',
      category: 'Oppo',
      price: 24990000,
      stock: 18,
      image:
        'https://images.unsplash.com/photo-1617997455090-270256d4dc22?q=80&w=1964&auto=format&fit=crop',
    },
  ];

  filteredProducts = [...this.products];
  searchTerm = '';
  sortBy = 'name';
  sortOrder = 'asc';

  search() {
    if (!this.searchTerm.trim()) {
      this.filteredProducts = [...this.products];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredProducts = this.products.filter(
        (product) =>
          product.name.toLowerCase().includes(term) ||
          product.category.toLowerCase().includes(term)
      );
    }

    this.sortProducts();
  }

  sortProducts() {
    this.filteredProducts.sort((a, b) => {
      let comparison = 0;

      if (this.sortBy === 'name') {
        comparison = a.name.localeCompare(b.name);
      } else if (this.sortBy === 'price') {
        comparison = a.price - b.price;
      } else if (this.sortBy === 'stock') {
        comparison = a.stock - b.stock;
      } else if (this.sortBy === 'category') {
        comparison = a.category.localeCompare(b.category);
      }

      return this.sortOrder === 'asc' ? comparison : -comparison;
    });
  }

  deleteProduct(id: number) {
    this.products = this.products.filter((product) => product.id !== id);
    this.search();
  }
}
