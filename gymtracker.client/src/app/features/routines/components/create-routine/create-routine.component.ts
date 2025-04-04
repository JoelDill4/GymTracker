import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoutineService } from '../../services/routine.service';

@Component({
  selector: 'app-create-routine',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal show d-block" tabindex="-1">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Create New Routine</h5>
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
                <button type="submit" class="btn btn-primary">Create</button>
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
export class CreateRoutineComponent {
  @Output() close = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  routine = {
    name: '',
    description: ''
  };

  constructor(private routineService: RoutineService) {}

  onSubmit(): void {
    this.routineService.createRoutine(this.routine).subscribe({
      next: () => {
        this.created.emit();
      },
      error: (error) => {
        console.error('Error creating routine:', error);
      }
    });
  }
} 