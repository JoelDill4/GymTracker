import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-base-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal fade show" style="display: block;" tabindex="-1">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header bg-primary text-white">
            <h5 class="modal-title">
              <i class="bi" [ngClass]="isEditing ? editIcon : createIcon"></i>
              {{ isEditing ? editTitle : createTitle }}
            </h5>
            <button type="button" class="btn-close btn-close-white" (click)="onCancel()"></button>
          </div>
          <div class="modal-body p-4">
            <ng-content></ng-content>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade show"></div>
  `,
  styleUrls: ['./base-modal.component.css']
})
export class BaseModalComponent {
  @Input() isEditing = false;
  @Input() createTitle = 'Create';
  @Input() editTitle = 'Edit';
  @Input() createIcon = 'bi-plus-circle';
  @Input() editIcon = 'bi-pencil';
  @Output() cancel = new EventEmitter<void>();

  onCancel(): void {
    this.cancel.emit();
  }
} 