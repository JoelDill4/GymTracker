import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal-footer',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="modal-footer border-0 px-0">
      <button type="button" class="btn btn-outline-secondary" (click)="onCancel()">
        <i class="bi bi-x-circle me-2"></i>Cancel
      </button>
      <button [type]="submitType" class="btn btn-primary" [disabled]="disabled">
        <i class="bi" [ngClass]="isEditing ? 'bi-save' : 'bi-check-circle'"></i>
        {{ isEditing ? 'Save Changes' : 'Create' }}
      </button>
    </div>
  `
})
export class ModalFooterComponent {
  @Input() isEditing = false;
  @Input() disabled = false;
  @Input() submitType: 'submit' | 'button' = 'submit';
  @Output() cancel = new EventEmitter<void>();

  onCancel(): void {
    this.cancel.emit();
  }
} 