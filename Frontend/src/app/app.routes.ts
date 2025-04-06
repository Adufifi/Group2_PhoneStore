import { Routes } from '@angular/router';
import { HomeComponent } from './user/home/home.component';
import { ProductsComponent as CustomerProductsComponent } from './user/products/products.component';
import { PromotionsComponent } from './user/promotions/promotions.component';
import { ContactComponent } from './user/contact/contact.component';
import { CartComponent } from './user/cart/cart.component';
import { NewsComponent } from './user/news/news.component';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { ProfileDetailComponent } from './user/profile-detail/profile-detail.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { ProductsComponent as AdminProductsComponent } from './admin/products/products.component';
import { OrdersComponent } from './admin/orders/orders.component';
import { CustomersComponent } from './admin/customers/customers.component';
import { ReportComponent } from './admin/report/report.component';
import { SettingsComponent } from './admin/settings/settings.component';
import { ForgotPasswordComponent } from './user/forgot-password/forgot-password.component';
import { AddProductComponent } from './admin/add-product/add-product.component';
import { AdminGuard } from './auth/guards/AdminGuard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'products', component: CustomerProductsComponent },
  { path: 'promotions', component: PromotionsComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  {
    path: 'cart',
    component: CartComponent,
  },
  { path: 'news', component: NewsComponent },

  // Auth routes - Các trang tài khoản
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'profile/:id',
    component: ProfileDetailComponent,
  },

  // Admin routes - Các trang admin
  { path: 'admin', component: DashboardComponent, canActivate: [AdminGuard] },
  {
    path: 'admin/products',
    component: AdminProductsComponent,
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/orders',
    component: OrdersComponent,
  },
  {
    path: 'admin/customers',
    component: CustomersComponent,
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/report',
    component: ReportComponent,
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/settings',
    component: SettingsComponent,
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/products/new',
    component: AddProductComponent,
    canActivate: [AdminGuard],
  },

  // Fallback route
  { path: '**', redirectTo: '' },
];
