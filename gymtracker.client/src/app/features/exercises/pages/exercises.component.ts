import { Component, OnInit } from '@angular/core';
import { Exercise } from '../models/exercise.model';
import { ExerciseService } from '../services/exercise.service';
import { CommonModule } from '@angular/common';
import { CreateExerciseComponent } from '../components/create-exercise/create-exercise.component';

@Component({
  selector: 'app-exercises',
  templateUrl: './exercises.component.html',
  styleUrls: ['./exercises.component.css'],
  standalone: true,
  imports: [CommonModule, CreateExerciseComponent]
})

export class ExercisesComponent implements OnInit {
  exercises: Exercise[] = [];
  loading = false;
  error: string | null = null;
  showCreateModal = false;

  constructor(private exerciseService: ExerciseService) { }

  ngOnInit(): void {
    this.loadExercises();
  }

  loadExercises(): void {
    this.loading = true;
    this.error = null;
    this.exerciseService.getExercises().subscribe({
      next: (data) => {
        this.exercises = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load exercises';
        this.loading = false;
      }
    });
  }

  onCreateExercise(): void {
    this.showCreateModal = true;
  }

  onCancel(): void {
    this.showCreateModal = false;
  }

  onExerciseCreated(exercise: Exercise): void {
    this.exercises.push(exercise);
    this.showCreateModal = false;
  }
} 
