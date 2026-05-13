import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExpenseService } from '../../services/expense';
import { ExpenseDetailsDto } from '../../models/expense.model';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-expense',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './expense.html',
  styleUrl: './expense.scss'
})
export class ExpenseComponent {
  private svc = inject(ExpenseService);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  message = signal('');
  success = false;
  editingId: number | null = null;
  returnState: any = null;

  form: ExpenseDetailsDto = this.emptyForm();

  constructor() {
    const state = window.history.state;  // FIXED - removed currentNavigation
    const record = state?.record;
    this.returnState = state?.returnState;

    if (record) {
      this.editingId = record.id;
      this.form = { ...record };
      this.form.expenseDate = record.expenseDate?.split('T')[0];
      this.form.entryAt = record.entryAt?.split('T')[0];
    }
  }

  submit() {
    if (this.loading()) return;  // ADDED - prevent double submit

    if (!this.form.expenseType || !this.form.amount || !this.form.expenseDate) {
      this.setMessage('Please fill all required fields', false);
      return;
    }

    this.loading.set(true);
    this.form.entryAt = new Date().toISOString().split('T')[0];
    this.form.userId = this.auth.userId;
    this.form.entryBy =  this.auth.userName;

    const obs = this.editingId  // MOVED obs to after validation
      ? this.svc.update(this.editingId, this.form)
      : this.svc.add(this.form);

    obs.subscribe({  // SINGLE subscribe only
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
          this.setMessage('Expense saved successfully!', true);
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

  emptyForm(): ExpenseDetailsDto {
    return {
      userId: this.auth.userId,
      expenseType: '',
      amount: 0,
      description: '',
      status: 'Pending',
      entryBy: '',
      entryAt: new Date().toISOString().split('T')[0]
    };
  }
}