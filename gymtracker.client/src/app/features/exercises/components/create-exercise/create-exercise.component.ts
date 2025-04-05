import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Exercise, CreateExerciseDto } from '../../models/exercise.model';
import { ExerciseService } from '../../services/exercise.service';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { BodyPart } from '../../../bodyParts/models/body-part.model';

@Component({
  selector: 'app-create-exercise',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-exercise.component.html',
  styleUrls: ['./create-exercise.component.css']
})
export class CreateExerciseComponent implements OnInit {
  @Output() cancel = new EventEmitter<void>();
  @Output() exerciseCreated = new EventEmitter<Exercise>();

  exercise: Exercise = {
    name: '',
    description: '',
    bodyPart: {
      id: '',
      name: ''
    }
  };

  bodyParts: BodyPart[] = [];
  selectedBodyPartId: string = '';

  constructor(
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) {}

  ngOnInit(): void {
    this.loadBodyParts();
  }

  loadBodyParts(): void {
    this.bodyPartService.getBodyParts().subscribe({
      next: (parts) => {
        this.bodyParts = parts;
      },
      error: (error) => {
        console.error('Error loading body parts:', error);
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onSubmit(): void {
    if (this.exercise.name && this.selectedBodyPartId) {
      const exerciseToCreate: CreateExerciseDto = {
        name: this.exercise.name,
        description: this.exercise.description,
        fk_bodypart: this.selectedBodyPartId
      };

      this.exerciseService.createExercise(exerciseToCreate).subscribe({
        next: (createdExercise) => {
          this.exerciseCreated.emit(createdExercise);
        },
        error: (error) => {
          console.error('Error creating exercise:', error);
        }
      });
    }
  }
} 
