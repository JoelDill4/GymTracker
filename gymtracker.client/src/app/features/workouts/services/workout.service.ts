import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Workout } from '../models/workout.model';
import { ExerciseSet } from '../../exercisesSets/models/exercise-set.model';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private apiUrl = '/api/Workout';

  constructor(private http: HttpClient) { }

  getWorkouts(): Observable<Workout[]> {
    return this.http.get<Workout[]>(this.apiUrl);
  }

  getWorkout(id: string): Observable<Workout> {
    return this.http.get<Workout>(`${this.apiUrl}/${id}`);
  }

  getWorkoutsByWorkoutDay(workoutDayId: string): Observable<Workout[]> {
    return this.http.get<Workout[]>(`${this.apiUrl}/workoutday/${workoutDayId}`);
  }

  getWorkoutsByDateRange(initDate?: string, endDate?: string): Observable<Workout[]> {
    let params = new HttpParams();
    if (initDate) {
      params = params.set('startDate', initDate);
    }
    if (endDate) {
      params = params.set('endDate', endDate);
    }
    return this.http.get<Workout[]>(`${this.apiUrl}/daterange`, { params });
  }

  createWorkout(workout: Pick<Workout, 'workoutDate' | 'observations'> & { workoutDayId?: string }): Observable<Workout> {
    return this.http.post<Workout>(this.apiUrl, {
      workoutDate: workout.workoutDate,
      observations: workout.observations,
      workoutDayId: workout.workoutDayId
    });
  }

  updateWorkout(id: string, workout: Pick<Workout, 'workoutDate' | 'observations'> & { workoutDayId?: string }): Observable<Workout> {
    return this.http.put<Workout>(`${this.apiUrl}/${id}`, {
      workoutDate: workout.workoutDate,
      observations: workout.observations,
      workoutDayId: workout.workoutDayId
    });
  }

  deleteWorkout(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getExerciseSetsFromWorkout(workoutId: string): Observable<ExerciseSet[]> {
    return this.http.get<ExerciseSet[]>(`${this.apiUrl}/getExerciseSets/${workoutId}`);
  }

  assignExerciseSetsOfExerciseToWorkout(workoutId: string, exerciseId: string, exerciseSets: ExerciseSet[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/assignExerciseSets/${workoutId}/${exerciseId}`, exerciseSets);
  }

  addExerciseSetToWorkout(workoutId: string, exerciseSet: ExerciseSet): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/addExerciseSet/${workoutId}`, exerciseSet);
  }

  removeExerciseSetFromWorkout(workoutId: string, exerciseSetId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/removeExerciseSet/${workoutId}/${exerciseSetId}`, {});
  }
} 
