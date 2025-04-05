import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { WorkoutDayService } from '../../../workoutDays/services/workoutday.service';
import { RoutineService } from '../../services/routine.service';
import { WorkoutDay } from '../../../workoutDays/models/workoutday.model';
import { Routine } from '../../models/routine.model';
import { switchMap } from 'rxjs/operators';
import { CreateWorkoutDayComponent } from '../../../workoutDays/components/create-workout-day/create-workout-day.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-routine-detail',
  templateUrl: './routine-detail.component.html',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    CreateWorkoutDayComponent
  ]
})
export class RoutineDetailComponent implements OnInit {
  routine: Routine | null = null;
  workoutDays: WorkoutDay[] = [];
  loading = false;
  error: string | null = null;
  showCreateModal = false;

  constructor(
    private route: ActivatedRoute,
    private routineService: RoutineService,
    private workoutDayService: WorkoutDayService
  ) { }

  ngOnInit(): void {
    this.route.params.pipe(
      switchMap(params => {
        const routineId = params['id'];
        return this.routineService.getRoutine(routineId);
      })
    ).subscribe({
      next: (routine) => {
        this.routine = routine;
        this.loadWorkoutDays(routine.id);
      },
      error: (error) => {
        console.error('Error loading routine:', error);
        this.error = 'Failed to load routine';
      }
    });
  }

  private loadWorkoutDays(routineId: string): void {
    this.loading = true;
    this.routineService.getWorkoutDaysByRoutine(routineId).subscribe({
      next: (workoutDays) => {
        this.workoutDays = workoutDays;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading workout days:', error);
        this.error = 'Failed to load workout days';
        this.loading = false;
      }
    });
  }

  onCreateWorkoutDay(): void {
    this.showCreateModal = true;
  }

  onCancelCreate(): void {
    this.showCreateModal = false;
  }

  onWorkoutDayCreated(workoutDay: WorkoutDay): void {
    this.workoutDays = [...this.workoutDays, workoutDay];
    this.showCreateModal = false;
  }

  deleteWorkoutDay(id: string): void {
    if (confirm('Are you sure you want to delete this workout day?')) {
      this.workoutDayService.deleteWorkoutDay(id).subscribe({
        next: () => {
          this.workoutDays = this.workoutDays.filter(w => w.id !== id);
        },
        error: (error) => {
          console.error('Error deleting workout day:', error);
          this.error = 'Failed to delete workout day';
        }
      });
    }
  }
} 
