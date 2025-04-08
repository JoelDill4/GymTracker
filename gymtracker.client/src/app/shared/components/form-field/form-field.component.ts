import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-field',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="mb-4">
      <label [for]="id" class="form-label fw-bold">{{ label }}</label>
      <div class="input-group">
        <span class="input-group-text">
          <i class="bi" [ngClass]="icon"></i>
        </span>
        <ng-content></ng-content>
      </div>
      <ng-content select="[error]"></ng-content>
    </div>
  `
})
export class FormFieldComponent {
  @Input() label = '';
  @Input() id = '';
  @Input() icon = '';
} 