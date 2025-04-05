import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  template: `
    <nav class="navbar navbar-expand-lg navbar-dark">
      <div class="container">
        <a class="navbar-brand" routerLink="/">
          <i class="bi bi-dumbbell me-2"></i>GymTracker
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item">
              <a class="nav-link" routerLink="/routines">
                <i class="bi bi-calendar-week me-1"></i>Routines
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" routerLink="/exercises">
                <i class="bi bi-list-check me-1"></i>Exercises
              </a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  `,
  styles: [`
    .navbar {
      background-color: var(--primary-color);
      padding: 1rem 0;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .navbar-brand {
      font-size: 1.5rem;
      font-weight: 600;
      color: white !important;
    }

    .nav-link {
      color: rgba(255, 255, 255, 0.8) !important;
      transition: color var(--transition-speed);
      padding: 0.5rem 1rem;
    }

    .nav-link:hover {
      color: white !important;
    }

    .navbar-toggler {
      border: none;
      padding: 0.5rem;
    }

    .navbar-toggler:focus {
      box-shadow: none;
    }
  `]
})
export class NavbarComponent {} 
