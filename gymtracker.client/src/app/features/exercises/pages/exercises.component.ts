import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Exercise } from '../models/exercise.model';
import { ExerciseService } from '../services/exercise.service';
import { BodyPartService } from '../../bodyParts/services/body-part.service';
import { BodyPart } from '../../bodyParts/models/body-part.model';
import { CreateExerciseComponent } from '../components/create-exercise/create-exercise.component';

@Component({
  selector: 'app-exercises',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, CreateExerciseComponent],
  template: `
    <div class="container fade-in">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="display-5 fw-bold">
          <i class="bi bi-list-check me-2"></i>Exercises
        </h1>
        <button class="btn btn-primary" (click)="onCreateExercise()">
          <i class="bi bi-plus-lg me-2"></i>Create Exercise
        </button>
      </div>

      <div class="row mb-4">
        <div class="col-md-6">
          <div class="input-group">
            <span class="input-group-text bg-white">
              <i class="bi bi-search"></i>
            </span>
            <input 
              type="text" 
              class="form-control" 
              placeholder="Search exercises..." 
              [(ngModel)]="searchTerm"
              (ngModelChange)="onSearch()"
              (keyup.enter)="onSearch()">
            <button 
              class="btn btn-outline-secondary" 
              type="button" 
              (click)="onSearch()">
              Search
            </button>
            <button 
              class="btn btn-outline-secondary" 
              type="button" 
              (click)="clearSearch()"
              *ngIf="searchTerm">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>
        </div>
        <div class="col-md-6">
          <div class="input-group">
            <span class="input-group-text bg-white">
              <i class="bi bi-person-arms-up"></i>
            </span>
            <select 
              class="form-select" 
              [(ngModel)]="selectedBodyPartId"
              (ngModelChange)="onBodyPartChange()">
              <option [ngValue]="null">All Body Parts</option>
              <option *ngFor="let bodyPart of bodyParts" [value]="bodyPart.id">
                {{ bodyPart.name }}
              </option>
            </select>
            <button 
              class="btn btn-outline-secondary" 
              type="button" 
              (click)="clearBodyPartFilter()"
              *ngIf="selectedBodyPartId">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>
        </div>
      </div>

      <div *ngIf="loading" class="text-center py-5">
        <div class="spinner-border" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>

      <div *ngIf="error" class="alert alert-danger">
        <i class="bi bi-exclamation-triangle me-2"></i>{{ error }}
      </div>

      <div *ngIf="!loading && !error">
        <div *ngIf="exercises.length === 0" class="alert alert-info">
          <i class="bi bi-info-circle me-2"></i>No exercises found. Click the Create Exercise button to add one.
        </div>
          
        <div class="row g-4">
          <div *ngFor="let exercise of exercises" class="col-md-6 col-lg-4">
            <div class="card h-100">
              <div class="card-body">
                <h5 class="card-title">
                  <i class="bi bi-dumbbell me-2"></i>{{ exercise.name }}
                </h5>
                <p class="card-text text-muted">{{ exercise.description }}</p>
                <div class="d-flex align-items-center mt-3">
                  <span class="badge bg-primary">
                    <i class="bi bi-person-arms-up me-1"></i>{{ exercise.bodyPart.name }}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <app-create-exercise
      *ngIf="showCreateModal"
      (cancel)="onCancelCreate()"
      (exerciseCreated)="onExerciseCreated($event)">
    </app-create-exercise>
  `,
  styles: [`
    h1 {
      color: var(--primary-color);
    }

    .card {
      transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

    .card:hover {
      transform: translateY(-5px);
      box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
    }

    .badge {
      font-size: 0.9rem;
      padding: 0.5rem 0.75rem;
      border-radius: 6px;
    }

    .alert {
      border-radius: 8px;
      padding: 1rem 1.25rem;
    }

    .spinner-border {
      width: 3rem;
      height: 3rem;
    }

    .input-group {
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    }

    .input-group-text {
      border-right: none;
    }

    .form-control, .form-select {
      border-left: none;
    }

    .form-control:focus, .form-select:focus {
      border-color: var(--border-color);
      box-shadow: none;
    }

    .btn-outline-secondary {
      border-color: var(--border-color);
    }

    .btn-outline-secondary:hover {
      background-color: var(--border-color);
      color: var(--text-color);
    }
  `]
})
export class ExercisesComponent implements OnInit, AfterViewInit {
  exercises: Exercise[] = [];
  bodyParts: BodyPart[] = [];
  loading = false;
  error: string | null = null;
  showCreateModal = false;
  searchTerm = '';
  selectedBodyPartId: string | null = null;

  constructor(
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) {
    console.log('ExercisesComponent constructed');
  }

  ngOnInit(): void {
    console.log('ExercisesComponent initialized');
    this.loadExercises();
    this.loadBodyParts();
  }

  ngAfterViewInit(): void {
    console.log('ExercisesComponent view initialized');
  }

  loadExercises(): void {
    console.log('Loading exercises...');
    this.loading = true;
    this.error = null;
    this.exerciseService.getExercises().subscribe({
      next: (data) => {
        console.log('Exercises loaded:', data);
        this.exercises = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading exercises:', error);
        this.error = 'Failed to load exercises';
        this.loading = false;
      }
    });
  }

  loadBodyParts(): void {
    this.bodyPartService.getBodyParts().subscribe({
      next: (data) => {
        this.bodyParts = data;
      },
      error: (error) => {
        console.error('Error loading body parts:', error);
      }
    });
  }

  onSearch(): void {
    console.log('Searching for:', this.searchTerm);
    if (!this.searchTerm.trim()) {
      this.loadExercises();
      return;
    }

    this.loading = true;
    this.error = null;
    this.exerciseService.searchExercisesByName(this.searchTerm).subscribe({
      next: (data) => {
        console.log('Search results:', data);
        this.exercises = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Search error:', error);
        this.error = 'Failed to search exercises';
        this.loading = false;
      }
    });
  }

  onBodyPartChange(): void {
    if (!this.selectedBodyPartId) {
      this.loadExercises();
      return;
    }

    this.loading = true;
    this.error = null;
    this.exerciseService.getExercisesByBodyPart(this.selectedBodyPartId).subscribe({
      next: (data) => {
        this.exercises = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error filtering by body part:', error);
        this.error = 'Failed to filter exercises by body part';
        this.loading = false;
      }
    });
  }

  clearSearch(): void {
    console.log('Clearing search');
    this.searchTerm = '';
    this.loadExercises();
  }

  clearBodyPartFilter(): void {
    this.selectedBodyPartId = null;
    this.loadExercises();
  }

  onCreateExercise(): void {
    this.showCreateModal = true;
  }

  onCancelCreate(): void {
    this.showCreateModal = false;
  }

  onExerciseCreated(exercise: Exercise): void {
    this.exercises.push(exercise);
    this.showCreateModal = false;
  }
} 
