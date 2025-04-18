import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Exercise } from '../../../exercises/models/exercise.model';
import { BodyPart } from '../../../bodyParts/models/body-part.model';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { ExerciseService } from '../../../exercises/services/exercise.service';
import { WorkoutDay } from '../../models/workoutday.model';
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
  exercises: Exercise[] = [];
  availableExercises: Exercise[] = [];
  filteredExercises: Exercise[] = [];
  selectedExerciseId: string | null = null;
  selectedBodyPartId: string | null = null;
  bodyParts: BodyPart[] = [];
  @Output() cancel = new EventEmitter<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
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
    this.workoutDayService.getExercisesByWorkoutDay(workoutDayId).subscribe(
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
    
    filtered = filtered.filter(exercise => 
      !this.exercises.some(added => added.id === exercise.id)
    );

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

    const exercise = this.filteredExercises.find(e => e.id === this.selectedExerciseId);
    if (exercise && exercise.id) {
      this.exercises.push(exercise);
      this.selectedExerciseId = null;
      this.updateFilteredExercises();
    }
  }

  onRemoveExercise(exerciseId: string): void {
    this.exercises = this.exercises.filter(e => e.id !== exerciseId);
    this.updateFilteredExercises();
  }

  onCancel(): void {
    const routineId = this.workoutDay?.routine?.id;

    this.router.navigate(['/routines', routineId]);
  }

  onSaveExercises(): void {
    if (!this.workoutDay) return;

    const exerciseIds = this.exercises.map(e => e.id).filter((id): id is string => id !== undefined);
    this.workoutDayService.assignExercisesToWorkoutDay(this.workoutDay.id, exerciseIds).subscribe(
      () => {
        const routineId = this.workoutDay?.routine?.id;
        this.router.navigate(['/routines', routineId]);
      },
      error => {
        console.error('Error saving exercises:', error);
      }
    );
  }

  viewWorkouts(): void {
    if (this.workoutDay) {
      const currentUrl = this.router.url;
      const workoutsUrl = currentUrl.replace('/exercises', '/workouts');
      this.router.navigateByUrl(workoutsUrl);
    }
  }
} 
