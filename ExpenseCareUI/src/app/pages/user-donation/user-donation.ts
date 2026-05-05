import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router'
import { AuthService } from '../../services/auth';

// tell TypeScript that Razorpay exists on window
declare var Razorpay: any;

@Component({
  selector: 'app-user-donate',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './user-donation.html',
  styleUrls: ['./user-donation.scss']
})
export class UserDonateComponent {
  private http = inject(HttpClient);
  private auth = inject(AuthService);
  router       = inject(Router);

  userId = this.auth.userId;
  donorName   = this.auth.userName; // pre-fill from session
  donorMobile = this.auth.userMobile;
  amount:  number | null = null;
  donationFor = '';
  notes       = '';
  isLoading   = false;
  message     = '';
  success     = false;

  quickAmounts = [100, 500, 1000, 5000];

  errors: any = {
    donorName:   null,
    donorMobile: null,
    amount:      null
  };

  onDonorNameInput(value: string) {
    this.donorName = value;
    if (!value) { this.errors.donorName = null; return; }
    this.errors.donorName = /^[a-zA-Z\s]+$/.test(value)
      ? null
      : 'Name can only contain letters';
  }

  onMobileInput(value: string) {
    // strip non-digits
    this.donorMobile = value.replace(/\D/g, '');
    if (!this.donorMobile) { this.errors.donorMobile = null; return; }
    this.errors.donorMobile = /^\d{10}$/.test(this.donorMobile)
      ? null
      : 'Enter a valid 10-digit mobile number';
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
    if (!this.amount || this.amount <= 0) {
      this.errors.amount = 'Please enter a valid amount';
      valid = false;
    }

    return valid;
  }

  initiatePayment() {
    if (!this.validate()) return;

    this.isLoading = true;
    this.message   = '';

    // Step 1 — create Razorpay order on backend
    this.http.post<any>('/api/razorpay/create-order', {
      amount:    this.amount,
      userId:    this.auth.userId,
      donorName: this.donorName
    }).subscribe({
      next: (order) => {
        this.isLoading = false;
        this.openRazorpay(order); // Step 2 — open checkout
      },
      error: () => {
        this.isLoading = false;
        this.success   = false;
        this.message   = 'Failed to initiate payment. Please try again.';
      }
    });
  }

  openRazorpay(order: any) {
    const options = {
      key:         order.keyId,
      amount:      order.amount * 100,  // paise
      currency:    'INR',
      name:        'ExpenseCare',
      description: this.donationFor || 'Donation',
      order_id:    order.orderId,
      prefill: {
        name:    this.donorName,
        contact: this.donorMobile
      },
      theme: { color: '#1976d2' },

      // Step 3 — on successful payment save donation to DB
      handler: (response: any) => {
        this.saveDonation(response.razorpay_payment_id);
      }
    };

    const rzp = new Razorpay(options);

    // handle payment failure
    rzp.on('payment.failed', () => {
      this.success = false;
      this.message = 'Payment failed. Please try again.';
    });

    rzp.open();
  }

  saveDonation(paymentId: string) {
    this.isLoading = true;

    const payload = {

      userId:     this.userId,
      donorName:    this.donorName,
      donorMobile:  this.donorMobile,
      amount:       this.amount,
      donationFor:  this.donationFor  || null,
      notes:        this.notes        || null,
      donationDate: this.getTodayLocalDate(),
      paymentMode:  'UPI',                // Razorpay default
      paymentReference: paymentId         // Razorpay payment ID as reference
    };

    this.http.post('/api/donation/add-donation', payload).subscribe({
      next: () => {
        this.isLoading = false;
        this.success   = true;
        this.message   = '🎉 Thank you! Your donation was successful.';

        // reset form
        this.amount     = null;
        this.donationFor = '';
        this.notes       = '';

        // go to my donations after 2 seconds
        setTimeout(() => this.router.navigate(['/user-donations']), 2000);
      },
      error: () => {
        this.isLoading = false;
        this.success   = false;
        // payment went through but save failed — important message
        this.message   = `Payment successful (ID: ${paymentId}) but record save failed. Please contact support.`;
      }
    });
  }

  getTodayLocalDate(): string {
    const today = new Date();
    const yyyy  = today.getFullYear();
    const mm    = String(today.getMonth() + 1).padStart(2, '0');
    const dd    = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`; // "2026-04-28" — no timezone, no time
  }


}