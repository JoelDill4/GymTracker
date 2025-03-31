import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BodyPart } from '../models/exercise.model';

@Injectable({
  providedIn: 'root'
})
export class BodyPartService {
  private apiUrl = '/api/BodyPart';

  constructor(private http: HttpClient) { }

  getBodyParts(): Observable<BodyPart[]> {
    return this.http.get<BodyPart[]>(this.apiUrl);
  }
} 
