import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-new-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button class="btn btn-primary new-button" (click)="onClick.emit()">
      <i class="bi bi-plus-lg me-2"></i>{{ text }}
    </button>
  `,
  styles: [`
    .new-button {
      background-color: var(--primary-color);
      border: none;
      border-radius: 25px;
      padding: 0.5rem 1.25rem;
      font-weight: 500;
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .new-button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .new-button:active {
      transform: translateY(0);
    }
  `]
})
export class NewButtonComponent {
  @Input() text: string = 'New';
  @Output() onClick = new EventEmitter<void>();
} 