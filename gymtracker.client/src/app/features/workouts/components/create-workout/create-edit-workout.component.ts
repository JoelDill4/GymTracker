import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Workout } from '../../models/workout.model';
import { WorkoutService } from '../../services/workout.service';
import { Exercise } from '../../../exercises/models/exercise.model';
import { ExerciseSet } from '../../../exercisesSets/models/exercise-set.model';

@Component({
  selector: 'app-create-edit-workout',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-edit-workout.component.html',
  styleUrl: './create-edit-workout.component.css'
})
export class CreateEditWorkoutComponent implements OnChanges {
  @Input() workoutDayId!: string;
  @Input() exercises: Exercise[] = []; 
  @Input() workoutToEdit?: Workout;
  @Input() singleExerciseId?: string;
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<Workout>();
  @Output() updated = new EventEmitter<Workout>();

  currentStep = 1;
  totalSteps = 0;
  workout: Partial<Workout> = {
    workoutDate: new Date(),
    observations: '',
    exerciseSets: []
  };
  loading = false;
  error = '';
  isEditMode = false;
  
  exerciseSets: { [key: string]: ExerciseSet[] } = {};
  numberOfSets: { [key: string]: number } = {};

  constructor(private workoutService: WorkoutService) {
  }

  getFormattedDate(): string {
    if (!this.workout.workoutDate) return '';
    const date = new Date(this.workout.workoutDate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  onDateChange(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.value) {
      this.workout.workoutDate = new Date(input.value);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['exercises']) {
      this.updateTotalSteps();
      this.initializeExerciseSets();
    }
    if (changes['workoutToEdit'] && this.workoutToEdit) {
      this.isEditMode = true;
      this.totalSteps = 1;
      this.workout = { ...this.workoutToEdit };
      this.initializeExerciseSetsFromWorkout();
      
      if (this.singleExerciseId) {
        const exerciseIndex = this.exercises.findIndex(e => e.id === this.singleExerciseId);
        if (exerciseIndex !== -1) {
          this.currentStep = exerciseIndex + 2;
          this.totalSteps = this.currentStep;
        }
      }
    }
  }

  private initializeExerciseSetsFromWorkout(): void {
    if (!this.workoutToEdit) return;
    
    this.exercises.forEach(exercise => {
      const sets = this.workoutToEdit!.exerciseSets.filter(set => set.exercise.id === exercise.id);
      this.exerciseSets[exercise.id] = [...sets];
      this.numberOfSets[exercise.id] = sets.length;
    });
  }

  private updateTotalSteps(): void {
    this.totalSteps = this.exercises.length + 1;
  }

  private initializeExerciseSets(): void {
    this.exercises.forEach(exercise => {
      this.exerciseSets[exercise.id] = [{
        id: crypto.randomUUID(),
        exerciseId: exercise.id,
        exercise: { id: exercise.id } as Exercise,
        order: 1,
        reps: 10,
        weight: 20,
        createdAt: new Date(),
        updatedAt: new Date(),
        isDeleted: false
      }];
      this.numberOfSets[exercise.id] = 1;
    });
  }

  updateSetsForExercise(exerciseId: string): void {
    const currentSets = this.exerciseSets[exerciseId] || [];
    const newNumberOfSets = Math.min(Math.max(this.numberOfSets[exerciseId] || 0, 0), 10);
    
    this.numberOfSets[exerciseId] = newNumberOfSets;
    
    while (currentSets.length < newNumberOfSets) {
      currentSets.push({
        id: crypto.randomUUID(),
        exerciseId: exerciseId,
        exercise: { id: exerciseId } as Exercise,
        order: currentSets.length + 1,
        reps: 0,
        weight: 0,
        createdAt: new Date(),
        updatedAt: new Date(),
        isDeleted: false
      });
    }
    
    while (currentSets.length > newNumberOfSets) {
      currentSets.pop();
    }
    
    this.exerciseSets[exerciseId] = currentSets;
  }

  getCurrentExercise(): Exercise | undefined {
    if (this.currentStep === 1) return undefined;
    return this.exercises[this.currentStep - 2];
  }

  getCurrentExerciseSets(): ExerciseSet[] {
    const currentExercise = this.getCurrentExercise();
    if (!currentExercise) return [];
    return this.exerciseSets[currentExercise.id] || [];
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
    if (this.isEditMode && this.workoutToEdit) {
      if (this.singleExerciseId) {
        this.workoutService.assignExerciseSetsOfExerciseToWorkout(
          this.workoutToEdit.id,
          this.singleExerciseId,
          this.exerciseSets[this.singleExerciseId] || []
        ).subscribe({
          next: () => {
            this.loading = false;
            this.updated.emit();
            this.cancel.emit();
          },
          error: (err) => {
            this.loading = false;
            this.error = err.error?.message || 'Failed to assign exercise sets of exercise';
          }
        });
      }
      else {
        this.workoutService.updateWorkout(this.workoutToEdit.id, {
          workoutDate: this.workout.workoutDate,
          observations: this.workout.observations || '',
          workoutDayId: this.workoutDayId
        }).subscribe({
          next: (updatedWorkout) => {
            this.loading = false;
            this.updated.emit(updatedWorkout);
            this.cancel.emit();
          },
          error: (err) => {
            this.loading = false;
            this.error = err.error?.message || 'Failed to update workout';
          }
        });
      }
    } else {
      this.workoutService.createWorkout({
        workoutDate: this.workout.workoutDate,
        observations: this.workout.observations || '',
        workoutDayId: this.workoutDayId
      }).subscribe({
        next: (createdWorkout) => {
          const exerciseSetPromises = Object.values(this.exerciseSets)
            .flat()
            .map(exerciseSet => {
              return this.workoutService.addExerciseSetToWorkout(
                createdWorkout.id,
                exerciseSet
              ).toPromise();
            });

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
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 
