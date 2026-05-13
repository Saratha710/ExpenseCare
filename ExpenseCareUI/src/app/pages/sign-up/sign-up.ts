import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './sign-up.html',
  styleUrl: './sign-up.scss'
})
export class SignUpComponent {
  private http   = inject(HttpClient);
  private router = inject(Router);
  private auth   = inject(AuthService);

  showPassword = false;
  showConfirm  = false;
  isLoading    = false;
  message      = '';
  success      = false;

  form = {
    fullName:        '',
    mobile:          '',
    userName:        '',
    password:        '',
    confirmPassword: '',
    email:           '',
    address :           ''
  };

errors: { [key: string]: string | null} = {};

  validate(field: string): boolean {
    switch (field) {
      case 'fullName':
        if (!this.form.fullName.trim()) {
          this.errors['fullName'] = 'Full name is required';
          return false;
        }
        delete this.errors['fullName'];
        break;

      case 'mobile':
        if (!/^\d{10}$/.test(this.form.mobile)) {
          this.errors['mobile'] = 'Enter a valid 10-digit mobile number';
          return false;
        }
        delete this.errors['mobile'];
        break;

      // case 'userName':
      //   if (!this.form.userName.trim()) {
      //     this.errors['userName'] = 'Username is required';
      //     return false;
      //   }
      //   delete this.errors['userName'];
      //   break;

      case 'password':
        if(!this.form.password) {
          delete this.errors['password'];
        }

        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
        if (!passwordRegex.test(this.form.password)) {
          this.errors['password'] = 'Password must be at least 8 characters long and include uppercase, lowercase, number, and special character';
          return false;
        }
        delete this.errors['password'];
        break;

      case 'confirmPassword':
        if(!this.form.confirmPassword) {
          delete this.errors['confirmPassword'];
        }
        if (this.form.password !== this.form.confirmPassword) {
          this.errors['confirmPassword'] = 'Passwords does not match';
          return false;
        }
        delete this.errors['confirmPassword'];
        break;
    }
    return true;
  }

  submit() {
    const fields = ['fullName', 'mobile', 'userName', 'password', 'confirmPassword','email'];
    const allValid = fields.every(f => this.validate(f));
    if (!allValid) return;

    this.isLoading = true;
    this.message   = 'Creating account...';

    this.http.post<{ data: { userId: string; name: string; role: string } }>(`${environment.apiUrl}/api/auth/register`, {
      fullName:  this.form.fullName,
      mobileNumber:    this.form.mobile,
      userName:  this.form.userName || this.form.fullName,
      email: this.form.email || null,
      address: this.form.address || null,
      password:  this.form.password
    }).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.auth.setSession({
          userId:   res.data.userId,
          userName: res.data.name,
          role:     res.data.role,
          mobile: this.form.mobile,
          address: this.form.address
        });
        this.router.navigate(['/user-home']);
      },
      error: (err) => {
        this.isLoading = false;
        this.success   = false;
        this.message   = err.error?.message ?? 'Registration failed. Please try again.';
      }
    });
  }

  allowOnlyDigits(e: KeyboardEvent) {
    if (!/\d/.test(e.key)) e.preventDefault();
  }

  onMobileInput() {
    const mobile = this.form.mobile ?? '';
    if(!mobile) {
      this.errors['mobile'] = 'Please enter a valid 10-digit mobile number';
      return;
    }
    this.errors['mobile'] = /^\d{10}$/.test(mobile) ? null 
    : 'Please enter a valid 10-digit mobile number';
  }

  onMobileBlur() {
    const mobile = this.form.mobile ?? '';
    if(!mobile) {
      this.errors['mobile'] = null;
      return;
    }
    this.onMobileInput();
  }

}
