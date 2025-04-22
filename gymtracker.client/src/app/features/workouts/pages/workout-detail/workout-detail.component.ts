import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Workout } from '../../models/workout.model';
import { WorkoutService } from '../../services/workout.service';
import { WorkoutDay } from '../../../workoutDays/models/workoutday.model';
import { WorkoutDayService } from '../../../workoutDays/services/workoutday.service';
import { Exercise } from '../../../exercises/models/exercise.model';
import { CreateEditWorkoutComponent } from '../../components/create-workout/create-edit-workout.component';
import { ExerciseSet } from '../../../exercisesSets/models/exercise-set.model';

@Component({
  selector: 'app-workout-detail',
  standalone: true,
  imports: [CommonModule, CreateEditWorkoutComponent],
  templateUrl: './workout-detail.component.html'
})
export class WorkoutDetailComponent implements OnInit {
  workoutDayId: string = '';
  workoutDay: WorkoutDay | null = null;
  workouts: Workout[] = [];
  exercises: Exercise[] = [];
  showCreateModal = false;
  workoutToEdit?: Workout;
  exerciseToEdit?: string;
  loading = false;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workoutService: WorkoutService,
    private workoutDayService: WorkoutDayService
  ) {}

  ngOnInit(): void {
    this.workoutDayId = this.route.snapshot.paramMap.get('workoutDayId') || '';
    if (this.workoutDayId) {
      this.loadWorkoutDay();
      this.loadWorkouts();
      this.loadExercises();
    }
  }

  getUniqueExercises(exerciseSets: ExerciseSet[]): Exercise[] {
    const uniqueExercises = new Map<string, Exercise>();
    exerciseSets.forEach(set => {
      if (set.exercise && !uniqueExercises.has(set.exercise.id)) {
        uniqueExercises.set(set.exercise.id, set.exercise);
      }
    });
    return Array.from(uniqueExercises.values());
  }

  getSetsForExercise(exerciseSets: ExerciseSet[], exerciseId: string): ExerciseSet[] {
    return exerciseSets
      .filter(set => set.exercise.id === exerciseId)
      .sort((a, b) => a.order - b.order);
  }

  loadWorkoutDay(): void {
    this.workoutDayService.getWorkoutDay(this.workoutDayId).subscribe({
      next: (workoutDay) => {
        this.workoutDay = workoutDay;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load workout day';
      }
    });
  }

  loadWorkouts(): void {
    this.workoutService.getWorkoutsByWorkoutDay(this.workoutDayId).subscribe({
      next: (workouts) => {
        this.workouts = workouts;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load workouts';
      }
    });
  }

  loadExercises(): void {
    this.workoutDayService.getExercisesByWorkoutDay(this.workoutDayId).subscribe({
      next: (exercises) => {
        this.exercises = exercises;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load exercises';
      }
    });
  }

  createWorkout(): void {
    this.workoutToEdit = undefined;
    this.showCreateModal = true;
  }

  onWorkoutCreated(workout: Workout): void {
    this.workouts = [...this.workouts, workout];
    this.showCreateModal = false;
  }

  onWorkoutUpdated(workout: Workout): void {
    const index = this.workouts.findIndex(w => w.id === workout.id);
    if (index !== -1) {
      this.workouts[index] = workout;
    }
    this.showCreateModal = false;
    this.workoutToEdit = undefined;
    this.exerciseToEdit = undefined;
  }

  editWorkout(workoutId: string): void {
    const workout = this.workouts.find(w => w.id === workoutId);
    if (workout) {
      this.workoutToEdit = workout;
      this.showCreateModal = true;
    }
  }

  deleteWorkout(workoutId: string): void {
    if (confirm('Are you sure you want to delete this workout?')) {
      this.workoutService.deleteWorkout(workoutId).subscribe({
        next: () => {
          this.workouts = this.workouts.filter(w => w.id !== workoutId);
        },
        error: (err) => {
          this.error = err.error?.message || 'Failed to delete workout';
        }
      });
    }
  }

  onCancelCreate(): void {
    this.showCreateModal = false;
    this.workoutToEdit = undefined;
    this.exerciseToEdit = undefined;
  }

  editExerciseSets(workoutId: string, exerciseId: string): void {
    const workout = this.workouts.find(w => w.id === workoutId);
    if (workout) {
      this.workoutToEdit = workout;
      this.exerciseToEdit = exerciseId;
      this.showCreateModal = true;
    }
  }

  goBack(): void {
    this.router.navigate(['/workout-days', this.workoutDayId, 'exercises']);
  }
} 
