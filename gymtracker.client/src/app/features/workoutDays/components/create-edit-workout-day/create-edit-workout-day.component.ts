import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkoutDay, CreateWorkoutDayDto } from '../../models/workoutday.model';
import { WorkoutDayService } from '../../services/workoutday.service';
import { BaseModalComponent } from '../../../../shared/components/base-modal/base-modal.component';
import { FormFieldComponent } from '../../../../shared/components/form-field/form-field.component';
import { ModalFooterComponent } from '../../../../shared/components/modal-footer/modal-footer.component';

@Component({
  selector: 'app-create-edit-workout-day',
  standalone: true,
  imports: [CommonModule, FormsModule, BaseModalComponent, FormFieldComponent, ModalFooterComponent],
  templateUrl: './create-edit-workout-day.component.html'
})
export class CreateWorkoutDayComponent implements OnInit {
  @Input() routineId!: string;
  @Input() workoutDayToEdit: WorkoutDay | null = null;
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<WorkoutDay>();
  @Output() updated = new EventEmitter<WorkoutDay>();

  workoutDay: CreateWorkoutDayDto = {
    name: '',
    description: '',
    routineId: ''
  };

  loading = false;
  error = '';
  isEditing = false;

  constructor(private workoutDayService: WorkoutDayService) {}

  ngOnInit(): void {
    this.workoutDay.routineId = this.routineId;
    if (this.workoutDayToEdit) {
      this.isEditing = true;
      this.workoutDay = {
        name: this.workoutDayToEdit.name,
        description: this.workoutDayToEdit.description || '',
        routineId: this.routineId
      };
    }
  }

  onSubmit(): void {
    if (!this.workoutDay.name) {
      this.error = 'Name is required';
      return;
    }

    if (!this.workoutDay.routineId) {
      this.error = 'Routine ID is required';
      return;
    }

    this.loading = true;
    this.error = '';

    if (this.isEditing && this.workoutDayToEdit) {
      this.workoutDayService.updateWorkoutDay(this.workoutDayToEdit.id, this.workoutDay).subscribe({
        next: (updatedWorkoutDay) => {
          this.loading = false;
          this.updated.emit(updatedWorkoutDay);
          this.cancel.emit();
        },
        error: (err) => {
          this.loading = false;
          this.error = err.error?.message || 'Failed to update workout day';
        }
      });
    } else {
      this.workoutDayService.createWorkoutDay(this.workoutDay).subscribe({
        next: (createdWorkoutDay) => {
          this.loading = false;
          this.created.emit(createdWorkoutDay);
          this.cancel.emit();
        },
        error: (err) => {
          this.loading = false;
          this.error = err.error?.message || 'Failed to create workout day';
        }
      });
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 
