import { Routes } from '@angular/router';
import { LoginComponent } from './module/client/login/login.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
