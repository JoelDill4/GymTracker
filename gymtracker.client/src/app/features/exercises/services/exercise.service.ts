import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Exercise } from '../models/exercise.model';

@Injectable({
  providedIn: 'root'
})
export class ExerciseService {
  private apiUrl = '/api/Exercise';

  constructor(private http: HttpClient) { }

  getExercises(): Observable<Exercise[]> {
    return this.http.get<Exercise[]>(this.apiUrl);
  }

  getExercise(id: string): Observable<Exercise> {
    return this.http.get<Exercise>(`${this.apiUrl}/${id}`);
  }

  searchExercisesByName(name: string): Observable<Exercise[]> {
    return this.http.get<Exercise[]>(`${this.apiUrl}/name/${name}`);
  }

  createExercise(exercise: Pick<Exercise, 'name' | 'description' | 'bodyPart'>): Observable<Exercise> {
    return this.http.post<Exercise>(this.apiUrl, {
      name: exercise.name,
      description: exercise.description,
      fk_bodyPart: exercise.bodyPart.id
    });
  }

  updateExercise(id: string, exercise: Pick<Exercise, 'name' | 'description' | 'bodyPart'>): Observable<Exercise> {
    return this.http.put<Exercise>(`${this.apiUrl}/${id}`, {
      name: exercise.name,
      description: exercise.description,
      fk_bodyPart: exercise.bodyPart.id
    });
  }

  deleteExercise(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getExercisesByBodyPart(bodyPartId: string): Observable<Exercise[]> {
    return this.http.get<Exercise[]>(`${this.apiUrl}/bodypart/${bodyPartId}`);
  }
} 
