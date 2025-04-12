import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkoutDay, CreateWorkoutDayDto, WorkoutDayExercise } from '../models/workoutday.model';
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

  createWorkoutDay(workoutDay: CreateWorkoutDayDto): Observable<WorkoutDay> {
    return this.http.post<WorkoutDay>(this.apiUrl, workoutDay);
  }

  updateWorkoutDay(id: string, workoutDay: Omit<WorkoutDay, 'id' | 'createdAt' | 'updatedAt'>): Observable<WorkoutDay> {
    return this.http.put<WorkoutDay>(`${this.apiUrl}/${id}`, workoutDay);
  }

  deleteWorkoutDay(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getExercisesFromWorkoutDay(workoutDayId: string): Observable<WorkoutDayExercise[]> {
    return this.http.get<WorkoutDayExercise[]>(`${this.apiUrl}/exercises/${workoutDayId}`);
  }

  addExerciseToWorkoutDay(workoutDayId: string, exerciseId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/exercises/${workoutDayId}/${exerciseId}`, null);
  }

  assignExercisesToWorkoutDay(workoutDayId: string, exerciseIds: string[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/exercises/${workoutDayId}`, exerciseIds);
  }

  removeExerciseFromWorkoutDay(workoutDayId: string, exerciseId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/exercises/${workoutDayId}/${exerciseId}`);
  }
} 
