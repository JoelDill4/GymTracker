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
  templateUrl: './create-edit-workout-day.component.html',
  styleUrl: './create-edit-workout-day.component.css'
})
export class CreateWorkoutDayComponent implements OnInit {
  @Input() routineId!: string;
  @Input() workoutDayToEdit: WorkoutDay | null = null;
  @Output() cancel = new EventEmitter<void>();
  @Output() workoutDayCreated = new EventEmitter<WorkoutDay>();
  @Output() workoutDayUpdated = new EventEmitter<WorkoutDay>();

  workoutDay: CreateWorkoutDayDto = {
    name: '',
    description: '',
    routineId: ''
  };

  loading = false;
  error: string | null = null;
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
    if (this.workoutDay.name && this.workoutDay.routineId) {
      this.loading = true;
      this.error = null;

      if (this.isEditing && this.workoutDayToEdit) {
        this.workoutDayService.updateWorkoutDay(this.workoutDayToEdit.id, this.workoutDay).subscribe({
          next: (updatedWorkoutDay) => {
            this.workoutDayUpdated.emit(updatedWorkoutDay);
            this.loading = false;
          },
          error: (error) => {
            console.error('Error updating workout day:', error);
            this.error = 'Failed to update workout day';
            this.loading = false;
          }
        });
      } else {
        this.workoutDayService.createWorkoutDay(this.workoutDay).subscribe({
          next: (createdWorkoutDay) => {
            this.workoutDayCreated.emit(createdWorkoutDay);
            this.loading = false;
          },
          error: (error) => {
            console.error('Error creating workout day:', error);
            this.error = 'Failed to create workout day';
            this.loading = false;
          }
        });
      }
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 
