import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Exercise } from '../../../exercises/models/exercise.model';
import { BodyPart } from '../../../bodyParts/models/body-part.model';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { ExerciseService } from '../../../exercises/services/exercise.service';
import { WorkoutDay, WorkoutDayExercise } from '../../models/workoutday.model';
import { WorkoutDayService } from '../../services/workoutday.service';

@Component({
  selector: 'app-workout-day-exercises',
  templateUrl: './workout-day-exercises.component.html',
  styleUrls: ['./workout-day-exercises.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class WorkoutDayExercisesComponent implements OnInit {
  workoutDay: WorkoutDay | null = null;
  exercises: WorkoutDayExercise[] = [];
  availableExercises: Exercise[] = [];
  filteredExercises: Exercise[] = [];
  selectedExerciseId: string | null = null;
  selectedBodyPartId: string | null = null;
  bodyParts: BodyPart[] = [];

  constructor(
    private route: ActivatedRoute,
    private workoutDayService: WorkoutDayService,
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) { }

  ngOnInit(): void {
    const workoutDayId = this.route.snapshot.paramMap.get('workoutDayId');
    if (workoutDayId) {
      this.loadWorkoutDay(workoutDayId);
      this.loadExercises(workoutDayId);
      this.loadBodyParts();
      this.loadAvailableExercises();
    }
  }

  private loadWorkoutDay(id: string): void {
    this.workoutDayService.getWorkoutDay(id).subscribe(
      workoutDay => this.workoutDay = workoutDay,
      error => console.error('Error loading workout day:', error)
    );
  }

  private loadExercises(workoutDayId: string): void {
    this.workoutDayService.getExercisesFromWorkoutDay(workoutDayId).subscribe(
      exercises => {
        this.exercises = exercises;
        this.updateFilteredExercises();
      },
      error => console.error('Error loading exercises:', error)
    );
  }

  private loadBodyParts(): void {
    this.bodyPartService.getBodyParts().subscribe({
      next: (data) => {
        this.bodyParts = data;
      },
      error: (error) => {
        console.error('Error loading body parts:', error);
      }
    });
  }

  private loadAvailableExercises(): void {
    this.exerciseService.getExercises().subscribe(
      exercises => {
        this.availableExercises = exercises;
        this.updateFilteredExercises();
      },
      error => console.error('Error loading available exercises:', error)
    );
  }

  private updateFilteredExercises(): void {
    let filtered = [...this.availableExercises];
    
    // Filter out already added exercises
    filtered = filtered.filter(exercise => 
      !this.exercises.some(added => added.id === exercise.id)
    );

    // Filter by selected body part
    if (this.selectedBodyPartId) {
      filtered = filtered.filter(exercise => 
        exercise.bodyPart.id === this.selectedBodyPartId
      );
    }

    this.filteredExercises = filtered;
  }

  onBodyPartChange(): void {
    this.updateFilteredExercises();
  }

  onAddExercise(): void {
    if (!this.selectedExerciseId || !this.workoutDay) return;

    this.workoutDayService.addExerciseToWorkoutDay(this.workoutDay.id, this.selectedExerciseId).subscribe(
      () => {
        this.loadExercises(this.workoutDay!.id);
        this.selectedExerciseId = null;
      },
      error => console.error('Error adding exercise:', error)
    );
  }

  onRemoveExercise(exerciseId: string): void {
    if (!this.workoutDay || !exerciseId) return;

    this.workoutDayService.removeExerciseFromWorkoutDay(this.workoutDay.id, exerciseId).subscribe(
      () => {
        this.loadExercises(this.workoutDay!.id);
      },
      error => console.error('Error removing exercise:', error)
    );
  }
} 
