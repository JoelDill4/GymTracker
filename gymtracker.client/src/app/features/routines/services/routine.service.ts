import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Routine } from '../models/routine.model';
import { WorkoutDay } from '../../workoutDays/models/workoutday.model';

@Injectable({
  providedIn: 'root'
})
export class RoutineService {
  private apiUrl = `/api/Routine`;

  constructor(private http: HttpClient) { }

  getRoutines(): Observable<Routine[]> {
    return this.http.get<Routine[]>(this.apiUrl);
  }

  getRoutine(id: string): Observable<Routine> {
    return this.http.get<Routine>(`${this.apiUrl}/${id}`);
  }

  getWorkoutDaysByRoutine(id: string): Observable<WorkoutDay[]> {
    return this.http.get<WorkoutDay[]>(`${this.apiUrl}/workoutDays/${id}`);
  }

  createRoutine(routine: Omit<Routine, 'id' | 'createdAt' | 'updatedAt' | 'isDeleted'>): Observable<Routine> {
    return this.http.post<Routine>(this.apiUrl, routine);
  }

  updateRoutine(id: string, routine: Omit<Routine, 'id' | 'createdAt' | 'updatedAt' | 'isDeleted'>): Observable<Routine> {
    return this.http.put<Routine>(`${this.apiUrl}/${id}`, routine);
  }

  deleteRoutine(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
} 
