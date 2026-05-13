import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';
import * as QRCode from 'qrcode';
import { environment } from '../../../environments/environment';

declare var Razorpay: any;

@Component({
  selector: 'app-user-donation',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './user-donation.html',
  styleUrls: ['./user-donation.scss']
})
export class UserDonateComponent implements OnInit {
  private http = inject(HttpClient);
  private auth = inject(AuthService);
  router       = inject(Router);

  userId       = this.auth.userId;
  donorName    = this.auth.userName;
  donorMobile  = this.auth.userMobile;
  donorAddress = this.auth.userAddress;
  amount: number | null = null;
  donationFor  = '';
  notes        = '';
  isLoading    = false;
  message      = '';
  success      = false;

  showPayment  = false;
  payMode: 'upi' | 'card' | 'bank' | 'none' = 'none';   // ← 'none' is the default
  upiId        = '';
  upiName      = '';
  qrDataUrl    = '';
  showQr       = false;
  accountHolderName = '';
bankName          = '';
bankAccountNumber = '';
bankIfscCode      = '';
hasBankDetails    = false;


  quickAmounts = [100, 500, 1000, 5000];

  errors: any = {
    donorName:   null,
    donorMobile: null,
    amount:      null
  };

  ngOnInit(): void {
    this.http.get<any>(`${environment.apiUrl}/api/upisettings`).subscribe({
      next: (res) => {
        if (res?.upiId) {
          this.upiId   = res.upiId;
          this.upiName = res.displayName ?? 'ExpenseCare';
          this.showQr  = true;
        }
        if (res?.bankAccountNumber) {
        this.accountHolderName = res.accountHolderName ?? '';
        this.bankName          = res.bankName          ?? '';
        this.bankAccountNumber = res.bankAccountNumber ?? '';
        this.bankIfscCode      = res.bankIfscCode      ?? '';
        this.hasBankDetails    = true;
      }
      }
    });
  }

  async generateQr(amount: number) {
    const upiUrl = `upi://pay?pa=${this.upiId}&pn=${encodeURIComponent(this.upiName)}&am=${amount}&cu=INR`;
    this.qrDataUrl = await QRCode.toDataURL(upiUrl, {
      width: 200,
      margin: 1,
      color: { dark: '#0a1628', light: '#ffffff' }
    });
  }

  onDonorNameInput(value: string) {
    this.donorName = value;
    if (!value) { this.errors.donorName = null; return; }
    this.errors.donorName = /^[a-zA-Z\s]+$/.test(value)
      ? null : 'Name can only contain letters';
  }

  onMobileInput(value: string) {
    this.donorMobile = value.replace(/\D/g, '');
    if (!this.donorMobile) {
      this.errors.donorMobile = 'Enter a valid 10-digit mobile number';
      return;
    }
    this.errors.donorMobile = /^\d{10}$/.test(this.donorMobile)
      ? null : 'Enter a valid 10-digit mobile number';
  }

  onMobileBlur() {
    if (!this.donorMobile) { this.errors.donorMobile = null; return; }
    this.onMobileInput(this.donorMobile);
  }

  onAmountInput() {
    const amt = this.amount;
    if (amt === null || amt === undefined || String(amt) === '') {
      this.errors.amount = 'Minimum donation amount is ₹50';
      return;
    }
    this.errors.amount = amt < 50 ? 'Minimum donation amount is ₹50' : null;
  }

  validate(): boolean {
    let valid = true;
    if (!this.donorName.trim()) {
      this.errors.donorName = 'Donor name is required';
      valid = false;
    }
    if (!this.donorMobile || !/^\d{10}$/.test(this.donorMobile)) {
      this.errors.donorMobile = 'Valid 10-digit mobile number is required';
      valid = false;
    }
    if (!this.amount || this.amount < 50) {
      this.errors.amount = 'Minimum donation amount is ₹50';
      valid = false;
    }
    return valid;
  }

  async initiatePayment() {
    if (!this.validate()) return;
    if (this.showQr) await this.generateQr(this.amount!);
    this.message     = '';
    this.payMode     = 'none';   // ← shows choice screen first
    this.showPayment = true;
  }

  confirmUpiPayment() {
    this.isLoading = true;
    this.http.post(`${environment.apiUrl}/api/donation/add-donation`, {
      userId:           this.userId,
      donorName:        this.donorName,
      donorMobile:      this.donorMobile,
      donorAddress:     this.donorAddress,
      amount:           this.amount,
      donationFor:      this.donationFor  || null,
      notes:            this.notes        || null,
      donationDate:     this.getTodayLocalDate(),
      paymentMode:      'UPI',
      paymentReference: null
    }).subscribe({
      next: () => {
        this.isLoading = false;
        this.success   = true;
        this.message   = '🎉 Thank you! Donation recorded. Pending admin approval.';
        setTimeout(() => this.router.navigate(['/user-donations'], { replaceUrl: true }), 2500);
      },
      error: (err) => {
        this.isLoading = false;
        this.message   = err.error?.message ?? 'Something went wrong.';
      }
    });
  }

  payViaRazorpay() {
    this.isLoading = true;
    this.message   = '';
    this.http.post<any>(`${environment.apiUrl}/api/razorpay/create-order`, {
      amount:    this.amount,
      userId:    this.auth.userId,
      donorName: this.donorName
    }).subscribe({
      next: (order) => {
        this.isLoading = false;
        this.openRazorpay(order);
      },
      error: () => {
        this.isLoading = false;
        this.message   = 'Failed to initiate payment. Please try again.';
      }
    });
  }

  openRazorpay(order: any) {
    const options = {
      key:         order.keyId,
      amount:      order.amount * 100,
      currency:    'INR',
      name:        'ExpenseCare',
      description: this.donationFor || 'Donation',
      order_id:    order.orderId,
      prefill: {
        name:    this.donorName,
        contact: this.donorMobile
      },
      theme: { color: '#7dd3fc' },
      config: {
        display: {
          blocks: {
            banks: { name: 'Pay via Bank', instruments: [{ method: 'netbanking' }] },
            card:  { name: 'Pay via Card', instruments: [{ method: 'card' }] }
          },
          sequence: ['block.banks', 'block.card'],
          preferences: { show_default_blocks: false }
        }
      },
      handler: (response: any) => {
        this.saveDonation(response.razorpay_payment_id);
      }
    };
    const rzp = new Razorpay(options);
    rzp.on('payment.failed', () => {
      this.success = false;
      this.message = 'Payment failed. Please try again.';
    });
    rzp.open();
  }

  saveDonation(paymentId: string) {
    this.isLoading = true;
    this.http.post(`${environment.apiUrl}/api/donation/add-donation`, {
      userId:           this.userId,
      donorName:        this.donorName,
      donorMobile:      this.donorMobile,
      donorAddress:     this.donorAddress,
      amount:           this.amount,
      donationFor:      this.donationFor  || null,
      notes:            this.notes        || null,
      donationDate:     this.getTodayLocalDate(),
      paymentMode:      'Razorpay',
      paymentReference: paymentId
    }).subscribe({
      next: () => {
        this.isLoading   = false;
        this.success     = true;
        this.message     = '🎉 Thank you! Your donation was successful.';
        this.amount      = null;
        this.donationFor = '';
        this.notes       = '';
        setTimeout(() => this.router.navigate(['/user-donations'], { replaceUrl: true }), 2000);
      },
      error: () => {
        this.isLoading = false;
        this.message   = `Payment successful (ID: ${paymentId}) but record save failed. Contact support.`;
      }
    });
  }

  copyUpiId() {
    navigator.clipboard.writeText(this.upiId);
  }

  getTodayLocalDate(): string {
    const today = new Date();
    const yyyy  = today.getFullYear();
    const mm    = String(today.getMonth() + 1).padStart(2, '0');
    const dd    = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
  confirmBankPayment() {
  this.isLoading = true;
  this.http.post(`${environment.apiUrl}/api/donation/add-donation`, {
    userId:           this.userId,
    donorName:        this.donorName,
    donorMobile:      this.donorMobile,
    donorAddress:     this.donorAddress,
    amount:           this.amount,
    donationFor:      this.donationFor  || null,
    notes:            this.notes        || null,
    donationDate:     this.getTodayLocalDate(),
    paymentMode:      'Bank Transfer',
    paymentReference: null
  }).subscribe({
    next: () => {
      this.isLoading = false;
      this.success   = true;
      this.message   = '🎉 Thank you! Donation recorded. Pending admin verification.';
      setTimeout(() => this.router.navigate(['/user-donations'], { replaceUrl: true }), 2500);
    },
    error: (err) => {
      this.isLoading = false;
      this.message   = err.error?.message ?? 'Something went wrong.';
    }
  });
}

copyText(value: string) {
  navigator.clipboard.writeText(value);
}

}