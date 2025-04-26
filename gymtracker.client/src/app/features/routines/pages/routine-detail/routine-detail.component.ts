import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { WorkoutDayService } from '../../../workoutDays/services/workoutday.service';
import { RoutineService } from '../../services/routine.service';
import { WorkoutDay } from '../../../workoutDays/models/workoutday.model';
import { Routine } from '../../models/routine.model';
import { switchMap } from 'rxjs/operators';
import { CreateWorkoutDayComponent } from '../../../workoutDays/components/create-edit-workout-day/create-edit-workout-day.component';
import { NewButtonComponent } from '../../../../shared/components/new-button/new-button.component';
import { ListCardComponent } from '../../../../shared/components/list-card/list-card.component';
import { EditButtonComponent } from '../../../../shared/components/edit-button/edit-button.component';
import { DeleteButtonComponent } from '../../../../shared/components/delete-button/delete-button.component';

@Component({
  selector: 'app-routine-detail',
  templateUrl: './routine-detail.component.html',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    CreateWorkoutDayComponent,
    NewButtonComponent,
    ListCardComponent,
    EditButtonComponent,
    DeleteButtonComponent
  ]
})
export class RoutineDetailComponent implements OnInit {
  routine: Routine | null = null;
  workoutDays: WorkoutDay[] = [];
  loading = false;
  error: string | null = null;
  showCreateModal = false;
  selectedWorkoutDay: WorkoutDay | null = null;

  constructor(
    private route: ActivatedRoute,
    private routineService: RoutineService,
    private workoutDayService: WorkoutDayService
  ) {}

  ngOnInit(): void {
    this.loadRoutine();
  }

  loadRoutine(): void {
    this.loading = true;
    this.error = null;

    this.route.params.pipe(
      switchMap(params => this.routineService.getRoutine(params['id']))
    ).subscribe({
      next: (routine) => {
        this.routine = routine;
        this.loadWorkoutDays();
      },
      error: (error) => {
        console.error('Error loading routine:', error);
        this.error = 'Failed to load routine';
        this.loading = false;
      }
    });
  }

  loadWorkoutDays(): void {
    if (!this.routine) return;

    this.routineService.getWorkoutDaysByRoutine(this.routine.id).subscribe({
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

  onEditWorkoutDay(workoutDay: WorkoutDay): void {
    this.selectedWorkoutDay = workoutDay;
    this.showCreateModal = true;
  }

  onModalClosed(): void {
    this.showCreateModal = false;
    this.selectedWorkoutDay = null;
  }

  onWorkoutDayCreated(workoutDay: WorkoutDay): void {
    this.workoutDays.push(workoutDay);
    this.showCreateModal = false;
  }

  onWorkoutDayUpdated(updatedWorkoutDay: WorkoutDay): void {
    const index = this.workoutDays.findIndex(w => w.id === updatedWorkoutDay.id);
    if (index !== -1) {
      this.workoutDays[index] = updatedWorkoutDay;
    }
    this.showCreateModal = false;
    this.selectedWorkoutDay = null;
  }

  deleteWorkoutDay(id: string): void {
    if (confirm('Are you sure you want to delete this workout day?')) {
      this.workoutDayService.deleteWorkoutDay(id).subscribe({
        next: () => {
          this.workoutDays = this.workoutDays.filter(w => w.id !== id);
        },
        error: (error) => {
          console.error('Error deleting workout day:', error);
        }
      });
    }
  }

  // Helper methods for ListCardComponent
  getWorkoutDayTitle = (workoutDay: WorkoutDay): string => workoutDay.name;
  getWorkoutDayDescription = (workoutDay: WorkoutDay): string | undefined => workoutDay.description;
  getWorkoutDayBadge = (workoutDay: WorkoutDay): { text: string; icon?: string; routerLink?: any[] } | undefined => ({
    text: 'Workouts',
    icon: 'bi-lightning',
    routerLink: ['/routines', this.routine?.id, 'workout-days', workoutDay.id, 'workouts']
  });
  getWorkoutDayRouterLink = (workoutDay: WorkoutDay): any[] => ['/routines', this.routine?.id, 'workout-days', workoutDay.id, 'exercises'];
  onWorkoutDayClick = (workoutDay: WorkoutDay): void => {
    // Handle workout day click if needed
  };
} 
