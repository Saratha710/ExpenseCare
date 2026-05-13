import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DonationService } from '../../services/donation';
import { ExpenseService }  from '../../services/expense';
import { AuthService }     from '../../services/auth';
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

  type          = signal<'donation' | 'expense'>('donation');
  records       = signal<any[]>([]);
  loading       = signal(false);
  toast         = signal('');
  toastSuccess  = true;
  actionLoading = false;

  // ── Selection ──────────────────────────────────────
  selectedIds = signal<number[]>([]);

  allSelected = computed(() =>
    this.records().length > 0 &&
    this.records().every(r => this.selectedIds().includes(r.id))
  );

  someSelected = computed(() =>
    this.selectedIds().length > 0 && !this.allSelected()
  );

  isSelected(id: number): boolean {
    return this.selectedIds().includes(id);
  }

  toggleOne(id: number) {
    const current = this.selectedIds();
    this.selectedIds.set(
      current.includes(id)
        ? current.filter(x => x !== id)
        : [...current, id]
    );
  }

  toggleSelectAll(event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    this.selectedIds.set(checked ? this.records().map(r => r.id) : []);
  }

  // ── Load ───────────────────────────────────────────
  ngOnInit() { this.load(); }

  load() {
    this.loading.set(true);
    this.selectedIds.set([]);  // clear selection on every reload

    const obs: Observable<any[]> = this.type() === 'donation'
      ? this.donationSvc.getPending()
      : this.expenseSvc.getPending();

    obs.subscribe({
      next: data => { this.records.set(data); this.loading.set(false); },
      error: ()   => { this.records.set([]);  this.loading.set(false); }
    });
  }

  // ── Approve selected ───────────────────────────────
  approveSelected() {
  const ids = this.selectedIds();
  if (ids.length === 0) return;

  this.showConfirm(`Do you want to approve ${ids.length} selected record(s)?`, () => {
    this.actionLoading = true;
    if (this.type() === 'donation') {
      this.donationSvc.approveAll(ids, this.auth.userName).subscribe({
        next: () => { this.actionLoading = false; this.showResult(`${ids.length} donation(s) approved`, true); this.load(); },
        error: () => { this.actionLoading = false; this.showResult('Approval failed. Please try again.', false); }
      });
    } else {
      let done = 0;
      ids.forEach(id => {
        this.expenseSvc.approve(id, this.auth.userName).subscribe({
          next: () => { done++; if (done === ids.length) { this.actionLoading = false; this.showResult(`${ids.length} expense(s) approved`, true); this.load(); }},
          error: () => { this.actionLoading = false; this.showResult('Approval failed. Please try again.', false); }
        });
      });
    }
  });
}

  // ── Reject selected ────────────────────────────────
  rejectSelected() {
  const ids = this.selectedIds();
  if (ids.length === 0) return;

  this.showConfirm(`Reject ${ids.length} selected record(s)?`, () => {
    this.actionLoading = true;
    let done = 0;
    ids.forEach(id => {
      const obs = this.type() === 'donation'
        ? this.donationSvc.reject(id, this.auth.userName)
        : this.expenseSvc.reject(id, this.auth.userName);
      obs.subscribe({
        next: () => { done++; if (done === ids.length) { this.actionLoading = false; this.showResult(`${ids.length} record(s) rejected`, true); this.load(); }},
        error: () => { this.actionLoading = false; this.showResult('Rejection failed. Please try again.', false); }
      });
    });
  });
}

  setType(t: 'donation' | 'expense') { this.type.set(t); this.load(); }

popup = signal<{
  show: boolean;
  mode: 'confirm' | 'result';
  message: string;
  success: boolean;
  pendingAction: (() => void) | null;
}>({ show: false, mode: 'result', message: '', success: true, pendingAction: null });

showConfirm(message: string, action: () => void) {
  this.popup.set({ show: true, mode: 'confirm', message, success: true, pendingAction: action });
}

showResult(message: string, ok: boolean) {
  this.popup.set({ show: true, mode: 'result', message, success: ok, pendingAction: null });
}

closePopup() {
  this.popup.set({ show: false, mode: 'result', message: '', success: true, pendingAction: null });
}

confirmAction() {
  const action = this.popup().pendingAction;
  this.closePopup();
  if (action) action();
}

  toggleSelectAll2() {
  if (this.allSelected()) {
    this.selectedIds.set([]);
  } else {
    this.selectedIds.set(this.records().map(r => r.id));
  }
}
}