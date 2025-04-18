import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routine } from '../../models/routine.model';
import { RoutineService } from '../../services/routine.service';
import { ModalFooterComponent } from '../../../../shared/components/modal-footer/modal-footer.component';
import { FormFieldComponent } from '../../../../shared/components/form-field/form-field.component';
import { BaseModalComponent } from '../../../../shared/components/base-modal/base-modal.component';

@Component({
  selector: 'app-create-edit-routine',
  standalone: true,
  imports: [CommonModule, FormsModule, ModalFooterComponent, FormFieldComponent, BaseModalComponent],
  templateUrl: './create-edit-routine.component.html'
})
export class CreateRoutineComponent implements OnInit {
  @Input() routineToEdit?: Routine;
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<Routine>();
  @Output() updated = new EventEmitter<Routine>();

  routine: Routine = {
    id: '',
    name: '',
    description: '',
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };
  loading = false;
  error = '';
  isEditing = false;

  constructor(private routineService: RoutineService) {}

  ngOnInit(): void {
    if (this.routineToEdit) {
      this.isEditing = true;
      this.routine = { ...this.routineToEdit };
    }
  }

  onSubmit(): void {
    if (!this.routine.name) {
      this.error = 'Name is required';
      return;
    }

    if (this.isEditing) {
      this.updateRoutine();
    } else {
      this.createRoutine();
    }
  }

  private createRoutine(): void {
    this.loading = true;
    this.error = '';
    this.routineService.createRoutine(this.routine).subscribe({
      next: (routine) => {
        this.loading = false;
        this.created.emit(routine);
        this.cancel.emit();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Failed to create routine';
      }
    });
  }

  private updateRoutine(): void {
    this.loading = true;
    this.error = '';
    this.routineService.updateRoutine(this.routine.id, this.routine).subscribe({
      next: (routine) => {
        this.loading = false;
        this.updated.emit(routine);
        this.cancel.emit();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Failed to update routine';
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 
