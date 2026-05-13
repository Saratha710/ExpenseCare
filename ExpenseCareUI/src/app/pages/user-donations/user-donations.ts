import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-user-donations',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-donations.html',
  styleUrls: ['./user-donations.scss']
})
export class UserDonationsComponent implements OnInit {
  private http   = inject(HttpClient);
  auth           = inject(AuthService);
  router         = inject(Router);

  donations: any[] = [];
  isLoading        = true;

  ngOnInit() {
    this.loadMyDonations();
  }

  loadMyDonations() {
    this.isLoading = true;
    const mobile= this.auth.userMobile;

    if(!mobile){
      this.donations = [];
      this.isLoading = false;
      return;
    }

    this.http.get<any[]>(`${environment.apiUrl}/api/donation/my-donations/${mobile}`)
      .subscribe({
        next: (data) => {
          this.donations = data;
          this.isLoading = false;
        },
        error: () => {
          this.donations = [];
          this.isLoading = false;
        }
      });
  }
}