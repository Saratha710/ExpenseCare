import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { adminGuard } from './guard/admin.guard';
import { userGuard }  from './guard/user.guard';
import { authGuard }  from './guard/auth.guard';

export const routes: Routes = [
  // public
  { path: '',           loadComponent: () => import('./pages/landing/landing').then(m => m.LandingComponent) },
  { path: 'login',      component: LoginComponent },
  { path: 'user-login', loadComponent: () => import('./pages/user-login/user-login').then(m => m.UserLoginComponent) },

  // admin/trustee — protected by authGuard
  { path: 'home',     loadComponent: () => import('./pages/home/home').then(m => m.HomeComponent),         canActivate: [authGuard] },
  { path: 'donation', loadComponent: () => import('./pages/donation/donation').then(m => m.DonationComponent), canActivate: [authGuard] },
  { path: 'expense',  loadComponent: () => import('./pages/expense/expense').then(m => m.ExpenseComponent),  canActivate: [adminGuard] },
  { path: 'reports',  loadComponent: () => import('./pages/reports/reports').then(m => m.ReportsComponent),  canActivate: [adminGuard] },
  { path: 'approve',  loadComponent: () => import('./pages/approve/approve').then(m => m.ApproveComponent),  canActivate: [adminGuard] },

  // user/donor — protected by userGuard
  { path: 'user-home',      loadComponent: () => import('./pages/user-home/user-home').then(m => m.UserHomeComponent),           canActivate: [userGuard] },
  { path: 'user-donations', loadComponent: () => import('./pages/user-donations/user-donations').then(m => m.UserDonationsComponent), canActivate: [userGuard] },
  { path: 'user-donate',    loadComponent: () => import('./pages/user-donation/user-donation').then(m => m.UserDonateComponent),     canActivate: [userGuard] },
  { path: 'sign-up',        loadComponent: () => import('./pages/sign-up/sign-up').then(m => m.SignUpComponent) },
  // catch all → landing
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}