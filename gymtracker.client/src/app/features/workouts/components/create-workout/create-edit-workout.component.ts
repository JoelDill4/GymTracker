import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Workout } from '../../models/workout.model';
import { WorkoutService } from '../../services/workout.service';
import { Exercise } from '../../../exercises/models/exercise.model';
import { ExerciseSet } from '../../../exercisesSets/models/exercise-set.model';
import { Routine } from '../../../routines/models/routine.model';
import { WorkoutDay } from '../../../workoutDays/models/workoutday.model';
import { RoutineService } from '../../../routines/services/routine.service';
import { WorkoutDayService } from '../../../workoutDays/services/workoutday.service';
import { ExerciseService } from '../../../exercises/services/exercise.service';
import { BodyPart } from '../../../bodyParts/models/body-part.model';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';

@Component({
  selector: 'app-create-edit-workout',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-edit-workout.component.html',
  styleUrl: './create-edit-workout.component.css'
})
export class CreateEditWorkoutComponent implements OnChanges {
  @Input() workoutDayId?: string;
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

  isRoutineRelated = true;
  routines: Routine[] = [];
  selectedRoutineId?: string;
  workoutDays: WorkoutDay[] = [];
  selectedWorkoutDayId?: string;
  showRoutineError = false;
  showWorkoutDayError = false;

  allExercises: Exercise[] = [];
  selectedExerciseIds: string[] = [];
  showExerciseError = false;

  filterName: string = '';
  filterBodyPartId: string = '';
  bodyParts: BodyPart[] = [];

  constructor(
    private workoutService: WorkoutService,
    private routineService: RoutineService,
    private workoutDayService: WorkoutDayService,
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) {
    if (!this.workoutDayId) {
      this.loadRoutines();
    }
    this.loadAllExercises();
    this.loadBodyParts();
  }

  private loadRoutines(): void {
    this.routineService.getRoutines().subscribe({
      next: (routines) => {
        this.routines = routines;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load routines';
      }
    });
  }

  onRoutineChange(event: Event): void {
    if (this.isEditMode) return;

    const select = event.target as HTMLSelectElement;
    const routineId = select.value;
    this.selectedRoutineId = routineId;
    this.selectedWorkoutDayId = undefined;
    this.workoutDays = [];
    
    if (routineId) {
      this.routineService.getWorkoutDaysByRoutine(routineId).subscribe({
        next: (workoutDays) => {
          this.workoutDays = workoutDays;
        },
        error: (err) => {
          this.error = err.error?.message || 'Failed to load workout days';
        }
      });
    }
  }

  onWorkoutDayChange(event: Event): void {
    if (this.isEditMode) return;

    const select = event.target as HTMLSelectElement;
    const workoutDayId = select.value;
    this.selectedWorkoutDayId = workoutDayId;

    this.workoutDayService.getExercisesByWorkoutDay(this.selectedWorkoutDayId).subscribe({
      next: (exercises) => {
        this.exercises = exercises;
        this.updateTotalSteps();
        this.initializeExerciseSets();
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load exercises';
      }
    });
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
    if (changes['workoutDayId'] && this.workoutDayId) {
      this.selectedWorkoutDayId = this.workoutDayId;
    }

    if (changes['workoutToEdit'] && this.workoutToEdit) {
      this.isEditMode = true;
      this.totalSteps = 1;
      this.workout = { ...this.workoutToEdit };

      if (this.workoutToEdit.workoutDay) {
        this.isRoutineRelated = true;
        this.selectedRoutineId = this.workoutToEdit.workoutDay.routine.id;
        this.selectedWorkoutDayId = this.workoutToEdit.workoutDay.id;

        this.routineService.getWorkoutDaysByRoutine(this.selectedRoutineId).subscribe({
          next: (workoutDays) => {
            this.workoutDays = workoutDays;
          },
          error: (err) => {
            this.error = err.error?.message || 'Failed to load workout days';
          }
        });
      }

      const workoutExerciseIds = this.workoutToEdit.exerciseSets.map(set => set.exercise.id);
      const allExerciseIds = Array.from(new Set([
        ...this.exercises.map(e => e.id),
        ...workoutExerciseIds
      ]));

      this.exercises = allExerciseIds.map(id => {
        return this.exercises.find(e => e.id === id) ||
               this.workoutToEdit!.exerciseSets.find(set => set.exercise.id === id)!.exercise;
      });

      this.initializeExerciseSetsFromWorkout();

      if (this.singleExerciseId) {
        const exerciseIndex = this.exercises.findIndex(e => e.id === this.singleExerciseId);
        if (exerciseIndex !== -1) {
          this.currentStep = exerciseIndex + 2;
          this.totalSteps = this.currentStep;
        }
      }
    }
    else if (changes['exercises']) {
      if (this.selectedWorkoutDayId == null) {
        this.totalSteps = 10;
      }
      else {
        this.updateTotalSteps();
      }

      this.initializeExerciseSets();
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
    if (this.isRoutineRelated) {
      this.totalSteps = 1 + this.exercises.length;
    } else {
      this.totalSteps = 2 + this.exercises.length;
    }
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
    const newNumberOfSets = this.numberOfSets[exerciseId] || 1;
    
    while (currentSets.length < newNumberOfSets) {
      currentSets.push({
        id: crypto.randomUUID(),
        exerciseId: exerciseId,
        exercise: { id: exerciseId } as Exercise,
        order: currentSets.length + 1,
        reps: 10,
        weight: 20,
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

    if (this.isRoutineRelated) {
      return this.exercises[this.currentStep - 2];
    }
    else {
      return this.exercises[this.currentStep - 3];
    }
  }

  getCurrentExerciseSets(): ExerciseSet[] {
    const currentExercise = this.getCurrentExercise();
    if (!currentExercise) return [];
    return this.exerciseSets[currentExercise.id] || [];
  }

  nextStep(): void {
    if (!this.isRoutineRelated) {
      if (this.currentStep === 1) {
        this.currentStep++;
        return;
      }
      if (this.currentStep === 2) {
        if (!this.selectedExerciseIds.length) {
          this.showExerciseError = true;
          return;
        }
        this.showExerciseError = false;
        this.exercises = this.allExercises.filter(e => this.selectedExerciseIds.includes(e.id));
        this.updateTotalSteps();
        this.initializeExerciseSets();
        this.currentStep++;
        return;
      }
    }

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
    if (!this.workout.workoutDate) {
      this.error = 'Workout date is required';
      return;
    }

    this.loading = true;
    this.error = '';
    this.showRoutineError = false;
    this.showWorkoutDayError = false;

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
          workoutDayId: this.isRoutineRelated ? this.selectedWorkoutDayId : undefined
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
        workoutDayId: this.isRoutineRelated ? this.selectedWorkoutDayId : undefined
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

  clampNumberOfSets(exerciseId: string): void {
    let value = Number(this.numberOfSets[exerciseId]);
    if (!value) value = 1;
    if (value < 1) value = 1;
    if (value > 10) value = 10;

    if (this.numberOfSets[exerciseId] !== value) {
      this.numberOfSets[exerciseId] = value === 10 ? 1 : 10;
      setTimeout(() => {
        this.numberOfSets[exerciseId] = value;
        this.updateSetsForExercise(exerciseId);
      });
    } else {
      this.numberOfSets[exerciseId] = value;
      this.updateSetsForExercise(exerciseId);
    }
  }

  loadAllExercises(): void {
    this.exerciseService.getExercises().subscribe({
      next: (exercises) => {
        this.allExercises = exercises;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load exercises';
      }
    });
  }

  onExerciseCheckboxChange(event: Event, exerciseId: string): void {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      if (!this.selectedExerciseIds.includes(exerciseId)) {
        this.selectedExerciseIds.push(exerciseId);
      }
    } else {
      this.selectedExerciseIds = this.selectedExerciseIds.filter(id => id !== exerciseId);
    }
  }

  loadBodyParts(): void {
    this.bodyPartService.getBodyParts().subscribe({
      next: (bodyParts) => {
        this.bodyParts = bodyParts;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load body parts';
      }
    });
  }

  get filteredExercises(): Exercise[] {
    return this.allExercises
      .filter(e => {
        const matchesName = this.filterName.trim() === '' || e.name.toLowerCase().includes(this.filterName.trim().toLowerCase());
        const matchesBodyPart = !this.filterBodyPartId || e.bodyPart?.id === this.filterBodyPartId;
        return matchesName && matchesBodyPart;
      })
      .sort((a, b) => {
        const aSelected = this.selectedExerciseIds.includes(a.id) ? 0 : 1;
        const bSelected = this.selectedExerciseIds.includes(b.id) ? 0 : 1;
        return aSelected - bSelected;
      });
  }
} 
