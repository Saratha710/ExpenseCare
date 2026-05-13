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

  inputValue:        string  = '';
  password:     string  = '';
  errorMessage: string  = '';
  isLoading:    boolean = false;
  showPassword: boolean = false;

  async login() {
    this.errorMessage = '';

    if (!this.inputValue.trim() ) {
      this.errorMessage = 'Please enter your user name, email or mobile number';
      return;
    }
    if (!this.password.trim()) {
      this.errorMessage = 'Password is required';
      return;
    }

    this.isLoading = true;

    try {
      const res = await firstValueFrom(
        this.auth.userLogin(this.inputValue.trim(), this.password)
      );

      this.auth.setSession({
        userId:   res.userId   ?? res.UserId,
        userName: res.name     ?? res.Name ?? res.email ?? res.Email,
        role:     res.role     ?? res.Role ?? 'User',
        mobile:  res.mobile   ?? res.Mobile ?? '',
        address: res.address ?? res.Address ?? '',
        accessToken: res.accessToken ?? '',
        refreshToken: res.refreshToken ?? ''
      });

      this.router.navigate(['/user-donation']);

    } catch (err: any) {
      this.errorMessage = err?.error?.message || 'Invalid email or password.';
    } finally {
      this.isLoading = false;
    }
  }

}