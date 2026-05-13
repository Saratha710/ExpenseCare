import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DonationService } from '../../services/donation';
import { DonationDetailsDto } from '../../models/donation.model';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-donation',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './donation.html',
  styleUrl: './donation.scss'
})
export class DonationComponent {
  private svc  = inject(DonationService);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading      = signal(false);
  message      = signal('');
  success      = false;
  editingId: number | null = null;
  returnState: any = null;

  amountError: string | null = null;
  mobileError: string | null = null;
  donorUserId: number | null = null;

  // Lookup state
  lookupLoading = false;
  donorFound: boolean | null = null;  // null = not searched yet

  quickAmounts = [100, 500, 1000, 5000];

  form: DonationDetailsDto = this.emptyForm();

  // All fields locked until mobile is entered (except when editing)
  get fieldsLocked(): boolean {
    return !this.editingId && this.donorFound === null;
  }

  constructor() {
    const state  = window.history.state;
    const record = state?.record;
    this.returnState = state?.returnState;

    if (record) {
      this.editingId  = record.id;
      this.form       = { ...record };
      this.form.donationDate = record.donationDate?.split('T')[0];
      this.form.entryAt      = record.entryAt?.split('T')[0];
      // When editing, treat as donor found so fields are editable
      this.donorFound = false;
    }
  }

  allowOnlyDigits(e: KeyboardEvent) {
    if (!/\d/.test(e.key)) e.preventDefault();
  }

  onMobileLookupInput() {
    // Reset lookup state when mobile changes
    this.donorFound  = null;
    this.mobileError = null;
    // Clear auto-filled fields if mobile changes
    if (!this.editingId) {
      this.form.donorName    = '';
      this.form.donorAddress = '';
    }
  }

  lookupDonor() {
    const mobile = (this.form.donorMobile ?? '').trim();
    if (!/^\d{10}$/.test(mobile)) {
      this.mobileError = 'Enter a valid 10-digit mobile number first';
      return;
    }
    this.mobileError  = null;
    this.lookupLoading = true;

    this.svc.getByMobile(mobile).subscribe({
      next: (res) => {
        this.lookupLoading = false;
        if (res) {
          this.form.donorName    = res.fullName  ?? '';
          this.form.donorAddress = res.address   ?? '';
          this.donorFound        = true;
          this.donorUserId      = res.userId    ?? null;
        } else {
          this.donorFound        = false;
          this.form.donorName    = '';
          this.form.donorAddress = '';
          this.donorUserId      = null;
        }
      },
      error: () => {
        this.lookupLoading = false;
        this.donorFound    = false;
        this.donorUserId      = null;
      }
    });
  }

  submit() {
    this.onAmountInput();
    if (this.amountError) return;
    if (this.loading()) return;

    if (!this.form.donorName?.trim() || !this.form.amount || !this.form.paymentMode) {
      this.setMessage('Please fill all required fields', false);
      return;
    }

    this.loading.set(true);
    this.form.entryAt  = new Date().toISOString().split('T')[0];
    this.form.userId   = this.donorFound && this.donorUserId 
                        ? this.donorUserId : this.auth.userId;
    this.form.entryBy  = this.auth.userName;

    if (!this.form.donationDate) {
      this.form.donationDate = new Date().toISOString().split('T')[0];
    }

    const obs = this.editingId
      ? this.svc.update(this.editingId, this.form)
      : this.svc.add(this.form);

    obs.subscribe({
      next: () => {
        this.loading.set(false);
        if (this.editingId) {
          this.router.navigate(['/reports'], {
            state: { returnState: this.returnState,
               editSuccess: true,
               editMessage: 'Donation updated successfully!'
             }
          });
        } else {
          this.setMessage('Donation saved successfully!', true);
          this.resetForm();
        }
      },
      error: () => {
        this.loading.set(false);
        this.setMessage('Failed. Please try again.', false);
      }
    });
  }

  resetForm() {
    this.editingId     = null;
    this.amountError   = null;
    this.mobileError   = null;
    this.donorFound    = null;
    this.lookupLoading = false;
    this.form          = this.emptyForm();
    this.donorUserId   = null;
  }

  setMessage(msg: string, ok: boolean) {
    this.success = ok;
    this.message.set(msg);
    setTimeout(() => this.message.set(''), 3000);
  }

  emptyForm(): DonationDetailsDto {
    return {
      userId:      this.auth.userId,
      donorName:   '',
      amount:      null as any,
      paymentMode: 'Cash',
      status:      'Pending',
      entryBy:     '',
      entryAt:     new Date().toISOString().split('T')[0]
    };
  }

  onAmountInput() {
    const amt = this.form.amount;
    if (amt === null || amt === undefined || String(amt) === '') {
      this.amountError = 'Minimum donation amount is ₹50';
      return;
    }
    this.amountError = amt < 50 ? 'Minimum donation amount is ₹50' : null;
  }
}