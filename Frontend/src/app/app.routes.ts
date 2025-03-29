import { Routes } from '@angular/router';

// Trang user (người dùng)
import { HomeComponent } from './user/home/home.component';
import { ProductsComponent as CustomerProductsComponent } from './user/products/products.component';
import { PromotionsComponent } from './user/promotions/promotions.component';
import { ContactComponent } from './user/contact/contact.component';
import { CartComponent } from './user/cart/cart.component';
import { NewsComponent } from './user/news/news.component';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { ForgotPasswordComponent } from './user/forgot-password/forgot-password.component';

// Trang admin
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { ProductsComponent as AdminProductsComponent } from './admin/products/products.component';
import { OrdersComponent } from './admin/orders/orders.component';
import { CustomersComponent } from './admin/customers/customers.component';
import { ReportComponent } from './admin/report/report.component';
import { SettingsComponent } from './admin/settings/settings.component';

// Guards
import { authGuard } from './page/guards/auth.guard';

export const routes: Routes = [
  // Main routes - Các trang người dùng
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'products', component: CustomerProductsComponent },
  { path: 'promotions', component: PromotionsComponent },
  { path: 'contact', component: ContactComponent },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [authGuard],
  },
  { path: 'news', component: NewsComponent },

  // Auth routes - Các trang tài khoản
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  // Admin routes - Các trang admin
  { path: 'admin', component: DashboardComponent },
  { path: 'admin/products', component: AdminProductsComponent },
  { path: 'admin/orders', component: OrdersComponent },
  { path: 'admin/customers', component: CustomersComponent },
  { path: 'admin/report', component: ReportComponent },
  { path: 'admin/settings', component: SettingsComponent },

  // Fallback route
  { path: '**', redirectTo: '' },
];
