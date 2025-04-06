import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WorkoutDayService } from '../../services/workoutday.service';
import { WorkoutDay } from '../../models/workoutday.model';
import { Exercise } from '../../../exercises/models/exercise.model';
import { ExerciseService } from '../../../exercises/services/exercise.service';

@Component({
  selector: 'app-workout-day-exercises',
  templateUrl: './workout-day-exercises.component.html',
  styleUrls: ['./workout-day-exercises.component.scss']
})
export class WorkoutDayExercisesComponent implements OnInit {
  workoutDay: WorkoutDay | null = null;
  exercises: Exercise[] = [];
  availableExercises: Exercise[] = [];
  selectedExerciseId: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private workoutDayService: WorkoutDayService,
    private exerciseService: ExerciseService
  ) { }

  ngOnInit(): void {
    const workoutDayId = this.route.snapshot.paramMap.get('id');
    if (workoutDayId) {
      this.loadWorkoutDay(workoutDayId);
      this.loadExercises(workoutDayId);
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
      exercises => this.exercises = exercises,
      error => console.error('Error loading exercises:', error)
    );
  }

  private loadAvailableExercises(): void {
    this.exerciseService.getExercises().subscribe(
      exercises => this.availableExercises = exercises,
      error => console.error('Error loading available exercises:', error)
    );
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
    if (!this.workoutDay) return;

    this.workoutDayService.removeExerciseFromWorkoutDay(this.workoutDay.id, exerciseId).subscribe(
      () => {
        this.loadExercises(this.workoutDay!.id);
      },
      error => console.error('Error removing exercise:', error)
    );
  }
} 