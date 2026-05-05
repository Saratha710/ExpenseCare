import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {

  private auth   = inject(AuthService);
  private http   = inject(HttpClient);
  private router = inject(Router);

  mobileNumber:    string  = '';
  otp:             string  = '';
  otpSent:         boolean = false;
  message:         string  = '';
  success:         boolean = false;
  isLoading:       boolean = false;
  validationError: string  = '';
  otpError:        string  = '';
  maxOtpAttempts:  number  = 3;
  otpAttempts:     number  = 0;
  resendTimer:     number  = 30;
  canResend:       boolean = false;

  private timerInterval: any;
  private MaxApiRetries: number = 3;

  async requestOtp() {
    const mobile = this.mobileNumber.trim();

    if (!mobile) {
      this.validationError = 'Mobile number is required';
      return;
    }
    if (!/^\d{10}$/.test(mobile)) {
      this.validationError = 'Enter a valid 10-digit mobile number';
      return;
    }

    this.validationError = '';
    this.isLoading = true;
    this.message   = 'Sending OTP...';
    this.success   = true;

    try {
      await this.retryRequest(
        () => this.http.post('/api/auth/request-otp', { mobileNumber: mobile }, { responseType: 'text' }),
        this.MaxApiRetries
      );
      this.message = 'OTP sent successfully!';
      this.otpSent = true;
      this.startResendTimer();
    } catch {
      this.success = false;
      this.message = 'Failed to send OTP. Please try again.';
    } finally {
      this.isLoading = false;
    }
  }

  async verifyOtp() {
    if (this.otpAttempts >= this.maxOtpAttempts) {
      this.message = 'Maximum OTP attempts exceeded. Request a new OTP.';
      this.success = false;
      return;
    }

    const otp = this.otp.trim();

    if (!otp) {
      this.otpError = 'OTP is required';
      return;
    }
    if (!/^\d{4,6}$/.test(otp)) {
      this.otpError = 'Enter a valid OTP';
      return;
    }

    this.otpError  = '';
    this.isLoading = true;
    this.message   = 'Verifying...';

    try {
      const res: any = await this.retryRequest(
        () => this.http.post<any>('/api/auth/verify-otp', { mobileNumber: this.mobileNumber, otp: this.otp }),
        this.MaxApiRetries
      );

      this.auth.setSession({
        userId:   res.userId   ?? res.UserId,
        userName: res.name     ?? res.Name ?? res.mobile ?? res.Mobile,
        role:     res.role     ?? res.Role ?? 'User'
      });

      this.success = true;
      this.message = 'Login successful! Redirecting...';
      setTimeout(() => this.router.navigate(['/home']), 1000);

    } catch {
      this.otpAttempts++;
      this.success = false;
      this.message = this.otpAttempts >= this.maxOtpAttempts
        ? 'Too many failed attempts. Please request a new OTP.'
        : `Invalid OTP. Attempts left: ${this.maxOtpAttempts - this.otpAttempts}`;
    } finally {
      this.isLoading = false;
    }
  }

  goBack() {
    this.otpSent        = false;
    this.otp            = '';
    this.otpError       = '';
    this.message        = '';
    this.mobileNumber   = '';
    this.validationError = '';
    this.otpAttempts    = 0;
    clearInterval(this.timerInterval);
  }

  goToDonorLogin() {
    this.router.navigate(['/user-login']);
  }

  startResendTimer() {
    this.canResend   = false;
    this.resendTimer = 30;
    this.timerInterval = setInterval(() => {
      this.resendTimer--;
      if (this.resendTimer <= 0) {
        clearInterval(this.timerInterval);
        this.canResend = true;
      }
    }, 1000);
  }

  resendOtp() {
    if (!this.canResend) return;
    this.requestOtp();
  }

  allowOnlyDigits(e: KeyboardEvent) {
    if (!/\d/.test(e.key)) e.preventDefault();
  }

  private retryRequest<T>(apiCall: () => any, retries: number): Promise<T> {
    return new Promise<T>((resolve, reject) => {
      const attempt = (remaining: number) => {
        apiCall().subscribe({
          next: (res: T) => resolve(res),
          error: (err: any) => {
            if (remaining > 0) attempt(remaining - 1);
            else reject(err);
          }
        });
      };
      attempt(retries);
    });
  }
}