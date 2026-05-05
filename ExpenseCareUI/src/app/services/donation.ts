// services/donation.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DonationDetailsDto } from '../models/donation.model';

@Injectable({ providedIn: 'root' })
export class DonationService {
  private http = inject(HttpClient);
  private api = '/api/donation';

  add(dto: DonationDetailsDto): Observable<DonationDetailsDto> {
    return this.http.post<DonationDetailsDto>(`${this.api}/add-donation`, dto);
  }

  getById(id: number): Observable<DonationDetailsDto> {
    return this.http.get<DonationDetailsDto>(`${this.api}/get-donation/${id}`);
  }

  getAll(): Observable<DonationDetailsDto[]> {
    return this.http.get<DonationDetailsDto[]>(`${this.api}/get-allDonations`);
  }

  update(id: number, dto:DonationDetailsDto): Observable<DonationDetailsDto>{
    return this.http.put<DonationDetailsDto>(`${this.api}/update-donation/${id}`,dto);
  }

  delete(id: number) : Observable<void>{
    return this.http.delete<void>(`${this.api}/delete-donation/${id}`);
  }
  getByMonth(year: number, month: number): Observable<DonationDetailsDto[]> {
  return this.http.get<DonationDetailsDto[]>(`${this.api}/by-month/${year}/${month}`);
}

getByYear(year: number): Observable<DonationDetailsDto[]> {
  return this.http.get<DonationDetailsDto[]>(`${this.api}/by-year/${year}`);
}

  getPending(): Observable<DonationDetailsDto[]> {
    return this.http.get<DonationDetailsDto[]>(`${this.api}/pending`);
  }

   approve(id: number, approvedBy: string): Observable<void> {
    return this.http.put<void>(`${this.api}/approve/${id}`, { approvedBy });
  }

}