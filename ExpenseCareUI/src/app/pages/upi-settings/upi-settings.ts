import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-upi-settings',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './upi-settings.html',
  styleUrl: './upi-settings.scss'
})
export class UpiSettingsComponent implements OnInit {
  private http = inject(HttpClient);

  // UPI
  upiId       = '';
  displayName = '';

  // Bank
  accountHolderName = '';
  bankName          = '';
  bankAccountNumber = '';
  bankIfscCode      = '';

  message   = signal('');
  success   = false;
  isLoading = false;

  ngOnInit() {
    this.http.get<any>('/api/upisettings').subscribe({
      next: (res) => {
        if (res) {
          this.upiId             = res.upiId             ?? '';
          this.displayName       = res.displayName       ?? '';
          this.accountHolderName = res.accountHolderName ?? '';
          this.bankName          = res.bankName          ?? '';
          this.bankAccountNumber = res.bankAccountNumber ?? '';
          this.bankIfscCode      = res.bankIfscCode      ?? '';
        }
      }
    });
  }

  save() {
    if (!this.upiId.trim()) {
      this.message.set('UPI ID is required');
      this.success = false;
      return;
    }

    this.isLoading = true;
    this.http.post<any>('/api/upisettings', {
      upiId:             this.upiId.trim(),
      displayName:       this.displayName.trim()       || null,
      accountHolderName: this.accountHolderName.trim() || null,
      bankName:          this.bankName.trim()          || null,
      bankAccountNumber: this.bankAccountNumber.trim() || null,
      bankIfscCode:      this.bankIfscCode.trim()      || null
    }).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.success   = true;
        this.message.set(res.message);
        setTimeout(() => this.message.set(''), 3000);
      },
      error: (err) => {
        this.isLoading = false;
        this.success   = false;
        this.message.set(err.error?.message ?? 'Failed to save');
      }
    });
  }
}