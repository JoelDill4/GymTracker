import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  template: `
    <nav class="sidenav">
      <div class="sidenav-content">
        <a class="brand" routerLink="/">
          <i class="bi bi-dumbbell me-2"></i>
          <span>GymTracker</span>
        </a>
        <ul class="nav-links">
          <li class="nav-item">
            <a class="nav-link" routerLink="/routines">
              <i class="bi bi-calendar-week"></i>
              <span>Routines</span>
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" routerLink="/exercises">
              <i class="bi bi-list-check"></i>
              <span>Exercises</span>
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" routerLink="/workouts">
              <i class="bi bi-lightning"></i>
              <span>Workouts</span>
            </a>
          </li>
        </ul>
      </div>
    </nav>
  `,
  styles: [`
    .sidenav {
      height: 100vh;
      width: 250px;
      position: fixed;
      top: 0;
      left: 0;
      background-color: var(--primary-color);
      padding: 1rem 0;
      box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);
      z-index: 1000;
    }

    .sidenav-content {
      display: flex;
      flex-direction: column;
      height: 100%;
    }

    .brand {
      font-size: 1.5rem;
      font-weight: 600;
      color: white;
      text-decoration: none;
      padding: 1rem 1.5rem;
      margin-bottom: 0;
      display: flex;
      align-items: center;
      border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    }

    .brand:hover {
      color: white;
    }

    .nav-links {
      list-style: none;
      padding: 0;
      margin: 1rem 0 0 0;
    }

    .nav-item {
      margin: 0;
    }

    .nav-link {
      color: rgba(255, 255, 255, 0.8);
      text-decoration: none;
      padding: 0.75rem 1.25rem;
      display: flex;
      align-items: center;
      transition: all var(--transition-speed);
    }

    .nav-link:hover {
      color: white;
      background-color: rgba(255, 255, 255, 0.1);
    }

    .nav-link i {
      margin-right: 1rem;
      font-size: 1.2rem;
    }

    @media (max-width: 768px) {
      .sidenav {
        width: 60px;
      }

      .brand span,
      .nav-link span {
        display: none;
      }

      .brand {
        justify-content: center;
        padding: 1rem;
      }

      .nav-link {
        justify-content: center;
        padding: 0.75rem;
      }

      .nav-link i {
        margin-right: 0;
      }
    }
  `]
})
export class NavbarComponent {} 
