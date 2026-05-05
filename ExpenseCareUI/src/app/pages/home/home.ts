// pages/home/home.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DonationService } from '../../services/donation';
import { ExpenseService } from '../../services/expense';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class HomeComponent implements OnInit {
  private donationSvc = inject(DonationService);
  private expenseSvc = inject(ExpenseService);

  donations = signal<any[]>([]);
  expenses = signal<any[]>([]);

  totalDonations = signal(0);
  totalExpenses = signal(0);
  netBalance = signal(0);
  donorCount = signal(0);

  ngOnInit() {
    this.loadDonations();
    this.loadExpenses();
  }

  loadDonations() {
    this.donationSvc.getAll().subscribe({  // replace 1 with actual userId
      next: (data) => {
        this.donations.set(data);
        const total = data.reduce((sum, d) => sum + d.amount, 0);
        this.totalDonations.set(total);
        this.donorCount.set(data.length);
        this.updateBalance();
      }
    });
  }

  loadExpenses() {
    this.expenseSvc.getAll().subscribe({  // replace 1 with actual userId
      next: (data) => {
        this.expenses.set(data);
        const total = data.reduce((sum, e) => sum + e.amount, 0);
        this.totalExpenses.set(total);
        this.updateBalance();
      }
    });
  }

  updateBalance() {
    this.netBalance.set(this.totalDonations() - this.totalExpenses());
  }

  // combine and sort both for recent transactions list
  get recentTransactions() {
    const d = this.donations().map(x => ({ ...x, type: 'Donation', name: x.donorName, date: x.donationDate , status: x.status}));
    const e = this.expenses().map(x => ({ ...x, type: 'Expense', name: x.expenseType, date: x.expenseDate , status: x.status}));
    return [...d, ...e]
      .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
      .slice(0, 5);  // show latest 5
  }
}