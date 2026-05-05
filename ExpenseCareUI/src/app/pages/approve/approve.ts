import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DonationService } from '../../services/donation';
import { ExpenseService } from '../../services/expense';
import { AuthService } from '../../services/auth';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-approve',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './approve.html',
  styleUrl: './approve.scss'
})
export class ApproveComponent implements OnInit {
  private donationSvc = inject(DonationService);
  private expenseSvc  = inject(ExpenseService);
  private auth        = inject(AuthService);

  type = signal<'donation' | 'expense'>('donation');
  records = signal<any[]>([]);
  loading = signal(false);
  toast = signal('');
  toastSuccess = true;

  ngOnInit() { this.load(); }

  load() {
    this.loading.set(true);

    const obs: Observable<any[]> = this.type() === 'donation'
      ? this.donationSvc.getPending()
      : this.expenseSvc.getPending();

    obs.subscribe({
      next: data => { this.records.set(data); this.loading.set(false); },
      error: ()   => { this.records.set([]);  this.loading.set(false); }
    });
  }

  approve(id: number, label: string) {
    if (!confirm(`Do you want to approve this ${this.type()} record?`)) return;

    const obs = this.type() === 'donation'
      ? this.donationSvc.approve(id, this.auth.userName)
      : this.expenseSvc.approve(id, this.auth.userName);

    obs.subscribe({
      next: () => {
        this.showToast(`The ${this.type()} has been approved successfully`, true);
        this.load();
      },
      error: () => this.showToast('Approval failed. Please try again.', false)
    });
  }

  setType(t: 'donation' | 'expense') { this.type.set(t); this.load(); }

  showToast(msg: string, ok: boolean) {
    this.toastSuccess = ok;
    this.toast.set(msg);
    setTimeout(() => this.toast.set(''), 3000);
  }
}