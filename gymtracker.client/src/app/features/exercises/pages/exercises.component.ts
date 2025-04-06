import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Exercise } from '../models/exercise.model';
import { ExerciseService } from '../services/exercise.service';
import { BodyPartService } from '../../bodyParts/services/body-part.service';
import { BodyPart } from '../../bodyParts/models/body-part.model';
import { CreateExerciseComponent } from '../components/create-edit-exercise/create-edit-exercise.component'

@Component({
  selector: 'app-exercises',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, CreateExerciseComponent],
  templateUrl: './exercises.component.html',
  styleUrl: './exercises.component.css'
})
export class ExercisesComponent implements OnInit, AfterViewInit {
  exercises: Exercise[] = [];
  bodyParts: BodyPart[] = [];
  loading = false;
  error: string | null = null;
  showCreateModal = false;
  searchTerm = '';
  selectedBodyPartId: string | null = null;
  selectedExercise: Exercise | null = null;

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
    this.selectedExercise = null;
  }

  onExerciseCreated(exercise: Exercise): void {
    this.exercises.push(exercise);
    this.showCreateModal = false;
  }

  onEditExercise(exercise: Exercise): void {
    this.selectedExercise = exercise;
    this.showCreateModal = true;
  }

  onExerciseUpdated(updatedExercise: Exercise): void {
    const index = this.exercises.findIndex(e => e.id === updatedExercise.id);
    if (index !== -1) {
      this.exercises[index] = updatedExercise;
    }
    this.showCreateModal = false;
    this.selectedExercise = null;
  }

  onDeleteExercise(id: string): void {
    if (confirm('Are you sure you want to delete this exercise?')) {
      this.loading = true;
      this.error = null;
      this.exerciseService.deleteExercise(id).subscribe({
        next: () => {
          console.log('Exercise deleted successfully');
          this.exercises = this.exercises.filter(e => e.id !== id);
          this.loading = false;
        },
        error: (error) => {
          console.error('Error deleting exercise:', error);
          this.error = 'Failed to delete exercise';
          this.loading = false;
        }
      });
    }
  }
} 
