// src/app/shared/header/header.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DonationService } from '../../services/donation';
import { ExpenseService } from '../../services/expense';
import { AuthService } from '../../services/auth';
import { forkJoin, filter } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class HeaderComponent implements OnInit {
  private donationSvc = inject(DonationService);
  private expenseSvc  = inject(ExpenseService);
  private router      = inject(Router);
  auth                = inject(AuthService);

  pendingCount = signal(0);
  isPreLoginPage = signal(false);

  // ALL pages that should NOT show the header
  private preLoginPages = ['/', '/login', '/user-login', ''];

  ngOnInit() {
    this.checkPage(this.router.url);

    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe((e: any) => {
        this.checkPage(e.urlAfterRedirects);
      });

    if (this.auth.isAdmin) this.loadPendingCount();
  }

  private checkPage(url: string) {
    // strip query params for comparison
    const path = url.split('?')[0];
    this.isPreLoginPage.set(this.preLoginPages.includes(path));
  }

  loadPendingCount() {
    forkJoin({
      donations: this.donationSvc.getPending(),
      expenses:  this.expenseSvc.getPending()
    }).subscribe({
      next: ({ donations, expenses }) => {
        this.pendingCount.set(donations.length + expenses.length);
      },
      error: () => this.pendingCount.set(0)
    });
  }

  goToApprovals() {
    this.router.navigate(['/approve']);
  }

  logout() {
    const isUser = this.auth.isUser;
    this.auth.clearSession();
    // redirect to landing page — not login
    this.router.navigate(['/']);
  }
}