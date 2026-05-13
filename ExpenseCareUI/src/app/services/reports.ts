// src/app/services/reports.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ExpenseRecord {
  id: number;
  userId?: number;
  amount?: number;
  status?: string;
  // add other fields your UI expects
}

@Injectable({
  providedIn: 'root'
})
export class ReportsService {
  private base = '/api/expenses'; // adjust to match your API
  private api = `${environment.apiUrl}/api/donation`;

  constructor(private http: HttpClient) {}

  getPending(): Observable<ExpenseRecord[]> {
    return this.http.get<ExpenseRecord[]>(`${this.base}/pending`);
  }

  deleteExpense(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }

  // optional: get single record
  getById(id: number): Observable<ExpenseRecord> {
    return this.http.get<ExpenseRecord>(`${this.base}/${id}`);
  }

  // optional: update
  updateExpense(id: number, payload: Partial<ExpenseRecord>): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}`, payload);
  }
}
