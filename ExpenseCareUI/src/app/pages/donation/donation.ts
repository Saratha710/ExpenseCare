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
  private svc = inject(DonationService);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  message = signal('');
  success = false;
  editingId: number | null = null;
  returnState: any = null;

  form: DonationDetailsDto = this.emptyForm();

  constructor() {
    const state = window.history.state;
    const record = state?.record;
    this.returnState = state?.returnState;

    if (record) {
      this.editingId = record.id;
      this.form = { ...record };
      this.form.donationDate = record.donationDate?.split('T')[0];
      this.form.entryAt = record.entryAt?.split('T')[0];
    }
  }

  submit() {
    if (this.loading()) return;

    if (!this.form.donorName || !this.form.amount || !this.form.paymentMode) {
      this.setMessage('Please fill all required fields', false);
      return;
    }

    this.loading.set(true);
    this.form.entryAt = new Date().toISOString().split('T')[0];
    this.form.userId = this.auth.userId;
    this.form.entryBy = this.form.entryBy?.trim() || this.auth.userName;

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
            state: { returnState: this.returnState }
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
    this.editingId = null;
    this.form = this.emptyForm();
  }

  setMessage(msg: string, ok: boolean) {
    this.success = ok;
    this.message.set(msg);
    setTimeout(() => this.message.set(''), 3000);
  }

  emptyForm(): DonationDetailsDto {
    return {
      userId: this.auth.userId,
      donorName: '',
      amount: 0,
      paymentMode: 'Cash',
      status: 'Pending',
      entryBy: '',
      entryAt: new Date().toISOString().split('T')[0]
    };
  }
}