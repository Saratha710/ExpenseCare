import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DonationService } from '../../services/donation';
import { ExpenseService } from '../../services/expense';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reports.html',
  styleUrl: './reports.scss'
})
export class ReportsComponent implements OnInit {
  
  private donationSvc = inject(DonationService);
  private expenseSvc = inject(ExpenseService);
  private router = inject(Router);

  type = signal<'donation' | 'expense'>('donation');
  filterMode = signal<'monthly' | 'yearly'>('monthly');
  selectedYear = (new Date().getFullYear());
  selectedMonth = (new Date().getMonth() + 1);

  allDonations: any[] = [];
  allExpenses: any[] = [];
  filtered = signal<any[]>([]);

  years = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i);
  months = [
    { val: 1, label: 'January' }, { val: 2, label: 'February' },
    { val: 3, label: 'March' }, { val: 4, label: 'April' },
    { val: 5, label: 'May' }, { val: 6, label: 'June' },
    { val: 7, label: 'July' }, { val: 8, label: 'August' },
    { val: 9, label: 'September' }, { val: 10, label: 'October' },
    { val: 11, label: 'November' }, { val: 12, label: 'December' }
  ];

  ngOnInit() {
    this.donationSvc.getAll().subscribe(d => { this.allDonations = d; this.applyFilter(); });
    this.expenseSvc.getAll().subscribe(e => { this.allExpenses = e; this.applyFilter(); });

    const returnState = window.history.state?.returnState;

  if (returnState) {
    this.type.set(returnState.type);
    this.filterMode.set(returnState.filterMode);
    this.selectedYear = returnState.selectedYear;
    this.selectedMonth = returnState.selectedMonth;
  }

  this.applyFilter();
  }

 applyFilter() {
  if (this.type() === 'donation') {
    const obs = this.filterMode() === 'monthly'
      ? this.donationSvc.getByMonth(this.selectedYear, this.selectedMonth)
      : this.donationSvc.getByYear(this.selectedYear);

    obs.subscribe({
      next: data => this.filtered.set(data),
      error: () => this.filtered.set([])
    });

  } 
  else {
    const obs = this.filterMode() === 'monthly'
      ? this.expenseSvc.getByMonth(this.selectedYear, this.selectedMonth)
      : this.expenseSvc.getByYear(this.selectedYear);

    obs.subscribe({
      next: data => this.filtered.set(data),
      error: () => this.filtered.set([])
    });
  }
}

  get total() {
    return this.filtered().reduce((sum, x) => sum + x.amount, 0);
  }

  setType(t: 'donation' | 'expense') { this.type.set(t); this.applyFilter(); }
  setMode(m: 'monthly' | 'yearly') { this.filterMode.set(m); this.applyFilter(); }

  onEdit(record: any) {
    const state = {
    record,
    returnState: {
      type: this.type(),
      filterMode: this.filterMode(),
      selectedYear: this.selectedYear,
      selectedMonth: this.selectedMonth
    }
  };
    if (this.type() === 'donation') {
    this.router.navigate(['/donation'], { state });
  } else {
    this.router.navigate(['/expense'], { state });
  }
}


toast = signal('');
toastSuccess = true;

showToast(msg: string, ok: boolean) {
  this.toastSuccess = ok;
  this.toast.set(msg);
  setTimeout(() => this.toast.set(''), 3000);
}

onDelete(id: number, name:string) {
  if (!confirm('Are you sure about deleting this record? The record will be permanently deleted.')) return;

  const obs = this.type() === 'donation'
    ? this.donationSvc.delete(id)
    : this.expenseSvc.delete(id);

    obs.subscribe({
    next: () => {
      this.showToast(`The ${this.type()} record has been deleted successfully.`, true);
      this.applyFilter();
    },
    error: () => this.showToast('Delete failed. Please try again.', false)
  });
}
}