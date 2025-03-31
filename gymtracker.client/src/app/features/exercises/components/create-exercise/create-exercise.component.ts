import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Exercise, BodyPart, CreateExerciseDto } from '../../models/exercise.model';
import { ExerciseService } from '../../services/exercise.service';

@Component({
  selector: 'app-create-exercise',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal fade show" style="display: block;" tabindex="-1">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Create Exercise</h5>
            <button type="button" class="btn-close" (click)="onCancel()"></button>
          </div>
          <div class="modal-body">
            <form (ngSubmit)="onSubmit()" #exerciseForm="ngForm">
              <div class="mb-3">
                <label for="name" class="form-label">Name</label>
                <input 
                  type="text" 
                  class="form-control" 
                  id="name" 
                  [(ngModel)]="exercise.name" 
                  name="name" 
                  required
                  minlength="3"
                  maxlength="100"
                  #name="ngModel">
                <div class="text-danger" *ngIf="name.invalid && (name.dirty || name.touched)">
                  <small *ngIf="name.errors?.['required']">Name is required</small>
                  <small *ngIf="name.errors?.['minlength']">Name must be at least 3 characters</small>
                  <small *ngIf="name.errors?.['maxlength']">Name cannot exceed 100 characters</small>
                </div>
              </div>
              <div class="mb-3">
                <label for="description" class="form-label">Description</label>
                <textarea 
                  class="form-control" 
                  id="description" 
                  [(ngModel)]="exercise.description" 
                  name="description" 
                  rows="3"
                  maxlength="500"
                  #description="ngModel"></textarea>
                <div class="text-danger" *ngIf="description.invalid && (description.dirty || description.touched)">
                  <small *ngIf="description.errors?.['maxlength']">Description cannot exceed 500 characters</small>
                </div>
              </div>
              <div class="mb-3">
                <label for="bodyPart" class="form-label">Body Part</label>
                <select 
                  class="form-select" 
                  id="bodyPart" 
                  [(ngModel)]="selectedBodyPart" 
                  name="bodyPart" 
                  required
                  #bodypart="ngModel">
                  <option value="">Select a body part</option>
                  <option *ngFor="let part of bodyParts" [value]="part">
                    {{ getBodyPartName(part) }}
                  </option>
                </select>
                <div class="text-danger" *ngIf="bodypart.invalid && (bodypart.dirty || bodypart.touched)">
                  <small *ngIf="bodypart.errors?.['required']">Body part is required</small>
                </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" (click)="onCancel()">Cancel</button>
                <button type="submit" class="btn btn-primary" [disabled]="!exerciseForm.valid">Create</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade show"></div>
  `
})
export class CreateExerciseComponent {
  @Output() cancel = new EventEmitter<void>();
  @Output() exerciseCreated = new EventEmitter<Exercise>();

  exercise: Exercise = {
    name: '',
    description: '',
    bodyPart: {
      id: '',
      name: ''
    }
  };

  selectedBodyPart: BodyPart | null = null;
  bodyParts = Object.values(BodyPart).filter(value => typeof value === 'number');

  constructor(private exerciseService: ExerciseService) {}

  getBodyPartName(part: BodyPart): string {
    return BodyPart[part];
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onSubmit(): void {
    if (this.exercise.name && this.selectedBodyPart !== null) {
      const exerciseToCreate: CreateExerciseDto = {
        name: this.exercise.name,
        description: this.exercise.description,
        fk_bodypart: this.selectedBodyPart.toString()
      };

      this.exerciseService.createExercise(exerciseToCreate).subscribe({
        next: (createdExercise) => {
          this.exerciseCreated.emit(createdExercise);
        },
        error: (error) => {
          console.error('Error creating exercise:', error);
        }
      });
    }
  }
} 
