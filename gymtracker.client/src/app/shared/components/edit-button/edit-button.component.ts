import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button class="btn btn-outline-primary rounded-3" (click)="onButtonClick($event)">
      <i class="bi bi-pencil"></i>
    </button>
  `,
  styles: [`
    .btn {
      padding: 0.5rem 0.75rem;
      font-size: 1.1rem;
      line-height: 1;
      display: flex;
      align-items: center;
      justify-content: center;
      min-width: 2.75rem;
    }
    
    .btn:hover {
      transform: translateY(-1px);
      transition: transform 0.2s;
    }
  `]
})
export class EditButtonComponent {
  @Output() onClick = new EventEmitter<void>();

  onButtonClick(event: MouseEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.onClick.emit();
  }
} 