import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkoutDay, CreateWorkoutDayDto } from '../../models/workoutday.model';
import { WorkoutDayService } from '../../services/workoutday.service';

@Component({
  selector: 'app-create-workout-day',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal fade show d-block" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Create Workout Day</h5>
            <button type="button" class="btn-close" (click)="onCancel()"></button>
          </div>
          <div class="modal-body">
            <form #workoutDayForm="ngForm" (ngSubmit)="onSubmit()">
              <div class="mb-3">
                <label for="name" class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  id="name"
                  name="name"
                  [(ngModel)]="workoutDay.name"
                  required
                  minlength="3"
                  maxlength="100"
                  #name="ngModel">
                <div *ngIf="name.invalid && (name.dirty || name.touched)" class="text-danger">
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
                  name="description"
                  [(ngModel)]="workoutDay.description"
                  maxlength="500"
                  rows="3"
                  #description="ngModel">
                </textarea>
                <div *ngIf="description.invalid && (description.dirty || description.touched)" class="text-danger">
                  <small *ngIf="description.errors?.['maxlength']">Description cannot exceed 500 characters</small>
                </div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="onCancel()">Cancel</button>
            <button 
              type="button" 
              class="btn btn-primary" 
              (click)="onSubmit()"
              [disabled]="workoutDayForm.invalid || loading">
              <span 
                *ngIf="loading" 
                class="spinner-border spinner-border-sm me-1" 
                role="status">
              </span>
              Create
            </button>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade show"></div>
  `,
  styles: [`
    :host {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      z-index: 1050;
    }
  `]
})
export class CreateWorkoutDayComponent {
  @Input() routineId!: string;
  @Output() cancel = new EventEmitter<void>();
  @Output() workoutDayCreated = new EventEmitter<WorkoutDay>();

  workoutDay: CreateWorkoutDayDto = {
    name: '',
    description: '',
    routineId: ''
  };

  loading = false;
  error: string | null = null;

  constructor(private workoutDayService: WorkoutDayService) {}

  ngOnInit(): void {
    if (this.routineId) {
      this.workoutDay.routineId = this.routineId;
    }
  }

  onSubmit(): void {
    if (!this.routineId) {
      this.error = 'Routine ID is required';
      return;
    }

    this.loading = true;
    this.error = null;

    this.workoutDayService.createWorkoutDay(this.workoutDay).subscribe({
      next: (createdWorkoutDay) => {
        this.loading = false;
        this.workoutDayCreated.emit(createdWorkoutDay);
      },
      error: (error) => {
        console.error('Error creating workout day:', error);
        this.error = 'Failed to create workout day';
        this.loading = false;
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 