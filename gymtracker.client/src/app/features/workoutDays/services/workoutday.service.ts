import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkoutDay } from '../models/workoutday.model';
import { Exercise } from '../../exercises/models/exercise.model';

@Injectable({
  providedIn: 'root'
})
export class WorkoutDayService {
  private apiUrl = '/api/WorkoutDay';

  constructor(private http: HttpClient) { }

  getWorkoutDays(): Observable<WorkoutDay[]> {
    return this.http.get<WorkoutDay[]>(this.apiUrl);
  }

  getWorkoutDay(id: string): Observable<WorkoutDay> {
    return this.http.get<WorkoutDay>(`${this.apiUrl}/${id}`);
  }

  getWorkoutDaysByRoutine(routineId: string): Observable<WorkoutDay[]> {
    return this.http.get<WorkoutDay[]>(`${this.apiUrl}/routine/${routineId}`);
  }

  createWorkoutDay(workoutDay: Pick<WorkoutDay, 'name' | 'description'> & { routineId: string }): Observable<WorkoutDay> {
    return this.http.post<WorkoutDay>(this.apiUrl, {
      name: workoutDay.name,
      description: workoutDay.description,
      routineId: workoutDay.routineId
    });
  }

  updateWorkoutDay(id: string, workoutDay: Pick<WorkoutDay, 'name' | 'description'> & { routineId: string }): Observable<WorkoutDay> {
    return this.http.put<WorkoutDay>(`${this.apiUrl}/${id}`, {
      name: workoutDay.name,
      description: workoutDay.description,
      routineId: workoutDay.routineId
    });
  }

  deleteWorkoutDay(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getExercisesByWorkoutDay(id: string): Observable<Exercise[]> {
    return this.http.get<Exercise[]>(`${this.apiUrl}/exercises/${id}`);
  }

  addExerciseToWorkoutDay(workoutDayId: string, exerciseId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/exercises/${workoutDayId}/${exerciseId}`, {});
  }

  assignExercisesToWorkoutDay(workoutDayId: string, exerciseIds: string[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/exercises/${workoutDayId}`, exerciseIds);
  }

  removeExerciseFromWorkoutDay(workoutDayId: string, exerciseId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/exercises/${workoutDayId}/${exerciseId}`);
  }
} 
