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
import { UserGuard } from './auth/guards/UserGuard';
import { EditProductComponent } from './admin/edit-product/edit-product.component';
import { ProductVariantsComponent } from './admin/product-variants/product-variants.component';
import { AddVariantComponent } from './admin/add-variant/add-variant.component';
import { ProductDetailComponent } from './user/product-detail/product-detail.component';
import { MyOrdersComponent } from './user/my-orders/my-orders.component';
import { CheckoutComponent } from './user/checkout/checkout.component';

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
    canActivate: [UserGuard],
  },
  { path: 'news', component: NewsComponent },

  // Auth routes - Các trang tài khoản
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'profile/:id',
    component: ProfileDetailComponent,
    canActivate: [UserGuard],
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
  { path: 'product-detail/:id', component: ProductDetailComponent },
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

  {
    path: 'admin/products/:id/edit',
    component: EditProductComponent,
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/product-variants/:id',
    component: ProductVariantsComponent,
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/products/:id/variants/new',
    component: AddVariantComponent,
    canActivate: [AdminGuard],
  },

  {
    path: 'order',
    component: MyOrdersComponent,
  },
  {
    path: 'checkout',
    component: CheckoutComponent,
  },

  // Fallback route
  { path: '**', redirectTo: '' },
];
