// services/auth.service.ts
import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private http = inject(HttpClient);

  private _userId = signal<number>(0);
  private _userName = signal<string>('');
  private _role = signal<string>('User');
  private _userMobile = signal<string>('');

  get userId() { return this._userId(); }
  get userName() { return this._userName(); }
  get role() {return this._role();}
  get userMobile() { return this._userMobile(); }

  get isAdmin() {return this._role() === 'Admin';}
  get isUser()   { return this._role() === 'User'; }

  userLogin(email: string, password: string) {
  return this.http.post<any>('/api/auth/user-login', { userName: email, password });
}

  setSession(data:any) {
    if(!data.userId) {
          console.error('userId is missing from response');
          return;
        }
      localStorage.setItem('userId',   data.userId);
      localStorage.setItem('userName', data.userName);
      localStorage.setItem('role',     data.role);

    this._userId.set(+data.userId);
    this._userName.set(data.userName);
    this._role.set(data.role ?? ''); 
       
  }



  loadFromStorage() {
    const id = localStorage.getItem('userId');
    const name = localStorage.getItem('userName');
    const role = localStorage.getItem('role');
    if (id) this._userId.set(+id);
    if (name) this._userName.set(name);
    if(role) this._role.set(role);
  }

  clearSession() {
    this._userId.set(0);
    this._userName.set('');
    this._role.set('');
    localStorage.removeItem('userId');
    localStorage.removeItem('userName');
    localStorage.removeItem('role');
  }

  isLoggedIn(): boolean {
    return this._userId() > 0;
  }


}