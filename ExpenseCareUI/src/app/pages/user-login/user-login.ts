import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './user-login.html',
  styleUrl: './user-login.scss'
})
export class UserLoginComponent {

  private auth   = inject(AuthService);
  private router = inject(Router);

  email:        string  = '';
  password:     string  = '';
  errorMessage: string  = '';
  isLoading:    boolean = false;

  async login() {
    this.errorMessage = '';

    if (!this.email.trim()) {
      this.errorMessage = 'Email is required';
      return;
    }
    if (!this.password.trim()) {
      this.errorMessage = 'Password is required';
      return;
    }

    this.isLoading = true;

    try {
      const res = await firstValueFrom(
        this.auth.userLogin(this.email.trim(), this.password)
      );

      this.auth.setSession({
        userId:   res.userId   ?? res.UserId,
        userName: res.name     ?? res.Name ?? res.email ?? res.Email,
        role:     res.role     ?? res.Role ?? 'User'
      });

      this.router.navigate(['/user-home']);

    } catch (err: any) {
      this.errorMessage = err?.error?.message || 'Invalid email or password.';
    } finally {
      this.isLoading = false;
    }
  }

  goToAdminLogin() {
    this.router.navigate(['/login']);
  }
}