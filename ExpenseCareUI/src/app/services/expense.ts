// services/expense.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExpenseDetailsDto } from '../models/expense.model';
import { environment } from '../../environments/environment.prod';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private http = inject(HttpClient);
  //private api = 'http://localhost:5104/api/expense';
  private api = `${environment.apiUrl}/api/expense`;

  add(dto: ExpenseDetailsDto): Observable<ExpenseDetailsDto> {
      return this.http.post<ExpenseDetailsDto>(`${this.api}/add-expense`, dto);
    }
  
    getById(id: number): Observable<ExpenseDetailsDto> {
      return this.http.get<ExpenseDetailsDto>(`${this.api}/get-expense/${id}`);
    }
  
    getAll(): Observable<ExpenseDetailsDto[]> {
      return this.http.get<ExpenseDetailsDto[]>(`${this.api}/get-allExpenses`);
    }
  
    update(id: number, dto:ExpenseDetailsDto): Observable<ExpenseDetailsDto>{
      return this.http.put<ExpenseDetailsDto>(`${this.api}/update-expense/${id}`,dto);
    }
  
    delete(id: number) : Observable<void>{
      return this.http.delete<void>(`${this.api}/delete-expense/${id}`);
    }
    getByMonth(year: number, month: number): Observable<ExpenseDetailsDto[]> {
    return this.http.get<ExpenseDetailsDto[]>(`${this.api}/by-month/${year}/${month}`);
  }
  
  getByYear(year: number): Observable<ExpenseDetailsDto[]> {
    return this.http.get<ExpenseDetailsDto[]>(`${this.api}/by-year/${year}`);
  }

  getPending(): Observable<ExpenseDetailsDto[]> {
    return this.http.get<ExpenseDetailsDto[]>(`${this.api}/pending`);
  }

  approve(id: number, approvedBy: string): Observable<void> {
    return this.http.put<void>(`${this.api}/approve/${id}`, { approvedBy });
  }
  reject(id: number, rejectedBy: string): Observable<void> {
  return this.http.put<void>(`${this.api}/reject/${id}`, { rejectedBy });
}
approveAll(ids: number[], approvedBy: string): Observable<void> {
  return this.http.put<void>(`${this.api}/approve-all`, { ids, approvedBy });
}
}