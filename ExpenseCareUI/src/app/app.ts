import { Component, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header';
import { AuthService } from './services/auth';


@Component({
  selector: 'app-root',
  standalone : true,
  imports: [RouterOutlet, HeaderComponent],
  template: `
    <app-header />
    <main class="main-content">
      <router-outlet />
    </main>
  `,
  styles: [`
    .main-content { padding: 2rem; background: #f5f5f5; min-height: calc(100vh - 56px); }
  `]
})
export class App {
  
  private auth = inject(AuthService);
    
  protected readonly title = signal('ExpenseCare');

  constructor() {
    this.auth.loadFromStorage();  //restore userId after refresh
  }
}
