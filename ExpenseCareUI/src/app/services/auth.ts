// services/auth.service.ts
import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private http = inject(HttpClient);

  private _userId = signal<number>(0);
  private _userName = signal<string>('');
  private _role = signal<string>('User');
  private _userMobile = signal<string>('');
  private _userAddress = signal<string>('');
  private _accessToken = signal<string>('');
  private _refreshToken = signal<string>('');

  get userId() { return this._userId(); }
  get userName() { return this._userName(); }
  get role() {return this._role();}
  get userMobile() { return this._userMobile(); }
  get userAddress() { return this._userAddress(); }
  get accessToken() { return this._accessToken(); }
  get refreshToken() { return this._refreshToken(); }

  get isAdmin() {return this._role() === 'Admin';}
  get isUser()   { return this._role() === 'User'; }
  get isTrustee() { return this._role() === 'Trustee'; }

  userLogin(identifier: string, password: string) {
  return this.http.post<any>(`${environment.apiUrl}/api/auth/user-login`, 
    { identifier: identifier, password });
}

  setSession(data:any) {
    if(!data.userId) {
          console.error('userId is missing from response');
          return;
        }
      localStorage.setItem('userId',   data.userId);
      localStorage.setItem('userName', data.userName);
      localStorage.setItem('role',     data.role);
      localStorage.setItem('userMobile', data.mobile ?? '');
      localStorage.setItem('userAddress', data.address ?? '');
      localStorage.setItem('accessToken', data.accessToken ?? '');
      localStorage.setItem('refreshToken', data.refreshToken ?? '');

    this._userId.set(+data.userId);
    this._userName.set(data.userName);
    this._role.set(data.role ?? ''); 
    this._userMobile.set(data.mobile ?? '');
    this._userAddress.set(data.address ?? '');
    this._accessToken.set(data.accessToken ?? '');
    this._refreshToken.set(data.refreshToken ?? '');

  }



  loadFromStorage() {
    const id = localStorage.getItem('userId');
    const name = localStorage.getItem('userName');
    const role = localStorage.getItem('role');
    const mobile = localStorage.getItem('userMobile');
    const address = localStorage.getItem('userAddress');
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');

    if (id) this._userId.set(+id);
    if (name) this._userName.set(name);
    if(role) this._role.set(role);
    if(mobile) this._userMobile.set(mobile);
    if(address) this._userAddress.set(address);
    if(accessToken) this._accessToken.set(accessToken);
    if(refreshToken) this._refreshToken.set(refreshToken);

  }

  clearSession() {
    this._userId.set(0);
    this._userName.set('');
    this._role.set('');
    this._userMobile.set('');
    this._userAddress.set('');
      this._accessToken.set('');
      this._refreshToken.set('');
    localStorage.removeItem('userId');
    localStorage.removeItem('userName');
    localStorage.removeItem('role');
    localStorage.removeItem('userMobile');
    localStorage.removeItem('userAddress');
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
  }

  isLoggedIn(): boolean {
    return this._userId() > 0;
  }

refreshAccessToken() {
  return this.http.post<any>(`${environment.apiUrl}/api/auth/refresh-otp`, {
    refreshToken: this._refreshToken()
  }).pipe(
    tap((res) => {
      // Update only the access token — keep everything else
      this._accessToken.set(res.accessToken);
      localStorage.setItem('accessToken', res.accessToken);
    })
  );
}

}