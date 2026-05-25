import { Routes } from '@angular/router';
import { LoginPage } from './pages/login/login.page';
import { OperatorPage } from './pages/operator/operator.page';

export const routes: Routes = [
  { path: 'login', component: LoginPage },
  { path: 'app/claude', component: OperatorPage },
  { path: 'claude', redirectTo: 'app/claude', pathMatch: 'full' },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
