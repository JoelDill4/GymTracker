import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoutineService } from '../../services/routine.service';
import { Routine } from '../../models/routine.model';

@Component({
  selector: 'app-edit-routine',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal show d-block" tabindex="-1">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Edit Routine</h5>
            <button type="button" class="btn-close" (click)="close.emit()"></button>
          </div>
          <div class="modal-body">
            <form (ngSubmit)="onSubmit()">
              <div class="mb-3">
                <label for="name" class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  id="name"
                  [(ngModel)]="routine.name"
                  name="name"
                  required
                >
              </div>
              <div class="mb-3">
                <label for="description" class="form-label">Description</label>
                <textarea
                  class="form-control"
                  id="description"
                  [(ngModel)]="routine.description"
                  name="description"
                  rows="3"
                ></textarea>
              </div>
              <div class="text-end">
                <button type="button" class="btn btn-secondary me-2" (click)="close.emit()">Cancel</button>
                <button type="submit" class="btn btn-primary">Save Changes</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop show"></div>
  `,
  styles: [`
    .modal-backdrop {
      z-index: 1040;
    }
    .modal {
      z-index: 1050;
    }
  `]
})
export class EditRoutineComponent {
  @Input() routine!: Routine;
  @Output() close = new EventEmitter<void>();
  @Output() updated = new EventEmitter<void>();

  constructor(private routineService: RoutineService) {}

  onSubmit(): void {
    this.routineService.updateRoutine(this.routine.id, {
      name: this.routine.name,
      description: this.routine.description
    }).subscribe({
      next: () => {
        this.updated.emit();
      },
      error: (error) => {
        console.error('Error updating routine:', error);
      }
    });
  }
} 