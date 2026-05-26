import { Component, signal, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header';
import { AuthService } from './services/auth';
import { LoggingService } from './services/logging';


@Component({
  selector: 'app-root',
  standalone : true,
  imports: [RouterOutlet, HeaderComponent],
  template: `
    <app-header />
    <main [class.main-content] = "auth.isLoggedIn()">
      <router-outlet />
    </main>
  `,
  styles: [`
    .main-content { 
      padding: 2rem; 
      background: #f5f5f5; 
      min-height: calc(100vh - 60px); }
  `]
})
export class App implements OnInit{
  
   auth = inject(AuthService);
   private logging = inject(LoggingService);
    
  protected readonly title = signal('ExpenseCare');

  constructor() {
    this.auth.loadFromStorage();  //restore userId after refresh
  }

  ngOnInit(){
     this.auth.loadFromStorage();
  }
  
}
