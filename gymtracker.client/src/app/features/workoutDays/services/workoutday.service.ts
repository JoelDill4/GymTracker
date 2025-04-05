import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkoutDay, CreateWorkoutDayDto } from '../models/workoutday.model';

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
} 
