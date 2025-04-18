import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Workout } from '../../models/workout.model';
import { WorkoutService } from '../../services/workout.service';
import { Exercise } from '../../../exercises/models/exercise.model';
import { ExerciseSet } from '../../../exercisesSets/models/exercise-set.model';

@Component({
  selector: 'app-create-workout',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-workout.component.html'
})
export class CreateWorkoutComponent implements OnChanges {
  @Input() workoutDayId!: string;
  @Input() exercises: Exercise[] = [];
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<Workout>();

  currentStep = 1;
  totalSteps = 0;
  workout: Partial<Workout> = {
    workoutDate: new Date(),
    observations: '',
    exerciseSets: []
  };
  loading = false;
  error = '';

  constructor(private workoutService: WorkoutService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['exercises']) {
      this.updateTotalSteps();
    }
  }

  ngOnInit(): void {
    this.updateTotalSteps();
  }

  private updateTotalSteps(): void {
    this.totalSteps = this.exercises.length + 1; // +1 for the initial step
  }

  nextStep(): void {
    if (this.currentStep < this.totalSteps) {
      this.currentStep++;
    }
  }

  previousStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  onSubmit(): void {
    if (!this.workout.workoutDate || !this.workoutDayId) {
      this.error = 'Workout date and workout day are required';
      return;
    }

    this.loading = true;
    this.error = '';

    // Create the workout first
    this.workoutService.createWorkout({
      workoutDate: this.workout.workoutDate,
      observations: this.workout.observations || '',
      workoutDayId: this.workoutDayId
    }).subscribe({
      next: (createdWorkout) => {
        // Add exercise sets
        const exerciseSetPromises = this.workout.exerciseSets?.map(exerciseSet => {
          if (exerciseSet.sets > 0 && exerciseSet.reps > 0) {
            return this.workoutService.addExerciseSetToWorkout(
              createdWorkout.id,
              exerciseSet
            ).toPromise();
          }
          return Promise.resolve();
        }) || [];

        Promise.all(exerciseSetPromises)
          .then(() => {
            this.loading = false;
            this.created.emit(createdWorkout);
            this.cancel.emit();
          })
          .catch(error => {
            this.loading = false;
            this.error = error.error?.message || 'Failed to add exercise sets';
          });
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Failed to create workout';
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 
