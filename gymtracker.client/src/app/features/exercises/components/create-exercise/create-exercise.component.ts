import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Exercise, BodyPart, CreateExerciseDto } from '../../models/exercise.model';
import { ExerciseService } from '../../services/exercise.service';
import { BodyPartService } from '../../services/body-part.service';

@Component({
  selector: 'app-create-exercise',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal fade show" style="display: block;" tabindex="-1">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header bg-primary text-white">
            <h5 class="modal-title">
              <i class="bi bi-plus-circle me-2"></i>Create Exercise
            </h5>
            <button type="button" class="btn-close btn-close-white" (click)="onCancel()"></button>
          </div>
          <div class="modal-body p-4">
            <form (ngSubmit)="onSubmit()" #exerciseForm="ngForm">
              <div class="mb-4">
                <label for="name" class="form-label fw-bold">Name</label>
                <div class="input-group">
                  <span class="input-group-text">
                    <i class="bi bi-tag"></i>
                  </span>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="name" 
                    [(ngModel)]="exercise.name" 
                    name="name" 
                    required
                    minlength="3"
                    maxlength="100"
                    placeholder="Enter exercise name"
                    #name="ngModel">
                </div>
                <div class="text-danger mt-1" *ngIf="name.invalid && (name.dirty || name.touched)">
                  <small *ngIf="name.errors?.['required']">Name is required</small>
                  <small *ngIf="name.errors?.['minlength']">Name must be at least 3 characters</small>
                  <small *ngIf="name.errors?.['maxlength']">Name cannot exceed 100 characters</small>
                </div>
              </div>

              <div class="mb-4">
                <label for="description" class="form-label fw-bold">Description</label>
                <div class="input-group">
                  <span class="input-group-text">
                    <i class="bi bi-card-text"></i>
                  </span>
                  <textarea 
                    class="form-control" 
                    id="description" 
                    [(ngModel)]="exercise.description" 
                    name="description" 
                    rows="3"
                    maxlength="500"
                    placeholder="Enter exercise description"
                    #description="ngModel"></textarea>
                </div>
                <div class="text-danger mt-1" *ngIf="description.invalid && (description.dirty || description.touched)">
                  <small *ngIf="description.errors?.['maxlength']">Description cannot exceed 500 characters</small>
                </div>
              </div>

              <div class="mb-4">
                <label for="bodyPart" class="form-label fw-bold">Body Part</label>
                <div class="input-group">
                  <span class="input-group-text">
                    <i class="bi bi-person-arms-up"></i>
                  </span>
                  <select 
                    class="form-select" 
                    id="bodyPart" 
                    [(ngModel)]="selectedBodyPartId" 
                    name="bodyPart" 
                    required
                    #bodypart="ngModel">
                    <option value="">Select a body part</option>
                    <option *ngFor="let part of bodyParts" [value]="part.id">
                      {{ part.name }}
                    </option>
                  </select>
                </div>
                <div class="text-danger mt-1" *ngIf="bodypart.invalid && (bodypart.dirty || bodypart.touched)">
                  <small *ngIf="bodypart.errors?.['required']">Body part is required</small>
                </div>
              </div>

              <div class="modal-footer border-0 px-0">
                <button type="button" class="btn btn-outline-secondary" (click)="onCancel()">
                  <i class="bi bi-x-circle me-2"></i>Cancel
                </button>
                <button type="submit" class="btn btn-primary" [disabled]="!exerciseForm.valid">
                  <i class="bi bi-check-circle me-2"></i>Create
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade show"></div>
  `,
  styles: [`
    .modal-content {
      border-radius: 12px;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

    .modal-header {
      border-radius: 12px 12px 0 0;
      padding: 1rem 1.5rem;
    }

    .modal-body {
      padding: 1.5rem;
    }

    .form-label {
      color: #495057;
      margin-bottom: 0.5rem;
    }

    .input-group-text {
      background-color: #f8f9fa;
      border-right: none;
    }

    .form-control, .form-select {
      border-left: none;
    }

    .form-control:focus, .form-select:focus {
      border-color: #dee2e6;
      box-shadow: none;
    }

    .input-group:focus-within {
      box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
      border-radius: 0.375rem;
    }

    .btn {
      padding: 0.5rem 1.25rem;
      border-radius: 6px;
      font-weight: 500;
    }

    .btn-primary {
      background-color: #0d6efd;
      border-color: #0d6efd;
    }

    .btn-primary:hover {
      background-color: #0b5ed7;
      border-color: #0a58ca;
    }

    .btn-outline-secondary {
      color: #6c757d;
      border-color: #6c757d;
    }

    .btn-outline-secondary:hover {
      background-color: #6c757d;
      border-color: #6c757d;
      color: white;
    }

    .text-danger {
      font-size: 0.875rem;
    }
  `]
})
export class CreateExerciseComponent implements OnInit {
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

  bodyParts: BodyPart[] = [];
  selectedBodyPartId: string = '';

  constructor(
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) {}

  ngOnInit(): void {
    this.loadBodyParts();
  }

  loadBodyParts(): void {
    this.bodyPartService.getBodyParts().subscribe({
      next: (parts) => {
        this.bodyParts = parts;
      },
      error: (error) => {
        console.error('Error loading body parts:', error);
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onSubmit(): void {
    if (this.exercise.name && this.selectedBodyPartId) {
      const exerciseToCreate: CreateExerciseDto = {
        name: this.exercise.name,
        description: this.exercise.description,
        fk_bodypart: this.selectedBodyPartId
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
