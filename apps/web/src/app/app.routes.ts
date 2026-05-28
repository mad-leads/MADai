import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home.page';
import { LoginPage } from './pages/login/login.page';
import { OperatorPage } from './pages/operator/operator.page';
import { authGuard, superAdminGuard } from './core/http/auth.guard';

export const routes: Routes = [
  { path: 'home', component: HomePage },
  { path: 'login', component: LoginPage },
  { path: 'dashboard', component: OperatorPage, canActivate: [authGuard] },
  { path: 'app/ai', redirectTo: 'ai', pathMatch: 'full' },
  { path: 'app/claude', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'claude', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'ai', canActivate: [superAdminGuard], loadComponent: () => import('./features/madcloud-ai/madcloud-ai.page').then((m) => m.MadcloudAiPage) },
  { path: '', component: HomePage, pathMatch: 'full' },
  { path: '**', redirectTo: 'home' }
];
