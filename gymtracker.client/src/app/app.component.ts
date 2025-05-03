import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { UpperNavbarComponent } from './shared/components/upper-navbar/upper-navbar.component';

@Component({
  selector: 'app-root',
  template: `
    <div class="app-container">
      <app-navbar></app-navbar>
      <main class="main-content">
        <app-upper-navbar></app-upper-navbar>
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styles: [`
    .app-container {
      min-height: 100vh;
      display: flex;
    }

    .main-content {
      flex: 1;
      padding: 2rem;
      margin-left: 250px;
      width: calc(100% - 250px);
      min-height: 100vh;
      background-color: var(--background-color);
    }

    @media (max-width: 768px) {
      .main-content {
        margin-left: 60px;
        width: calc(100% - 60px);
      }
    }
  `],
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, UpperNavbarComponent]
})
export class AppComponent implements OnInit {
  title = 'gymtracker';

  constructor() {
  }

  ngOnInit() {
    console.log('AppComponent initialized');
  }
} 
