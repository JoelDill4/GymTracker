import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Workout } from '../../models/workout.model';
import { WorkoutService } from '../../services/workout.service';
import { WorkoutDay } from '../../../workoutDays/models/workoutday.model';
import { WorkoutDayService } from '../../../workoutDays/services/workoutday.service';
import { Exercise } from '../../../exercises/models/exercise.model';
import { CreateWorkoutComponent } from '../../components/create-workout/create-workout.component';

@Component({
  selector: 'app-workout-detail',
  standalone: true,
  imports: [CommonModule, CreateWorkoutComponent],
  templateUrl: './workout-detail.component.html'
})
export class WorkoutDetailComponent implements OnInit {
  workoutDayId: string = '';
  workoutDay: WorkoutDay | null = null;
  workouts: Workout[] = [];
  exercises: Exercise[] = [];
  showCreateModal = false;
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
    this.showCreateModal = true;
  }

  onWorkoutCreated(workout: Workout): void {
    this.workouts = [...this.workouts, workout];
    this.showCreateModal = false;
  }

  onCancelCreate(): void {
    this.showCreateModal = false;
  }

  editWorkout(workoutId: string): void {
    this.router.navigate(['/workouts', workoutId, 'edit']);
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

  goBack(): void {
    this.router.navigate(['/workout-days', this.workoutDayId, 'exercises']);
  }
} 
