import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';

@Component({
  selector: 'app-root',
  template: `
    <div class="app-container">
      <app-navbar></app-navbar>
      <main class="main-content">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styles: [`
    .app-container {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
    }

    .main-content {
      flex: 1;
      padding: 2rem 0;
    }
  `],
  standalone: true,
  imports: [RouterOutlet, NavbarComponent]
})
export class AppComponent implements OnInit {
  title = 'gymtracker';

  constructor() {
    console.log('AppComponent constructed');
  }

  ngOnInit() {
    console.log('AppComponent initialized');
  }
} 
