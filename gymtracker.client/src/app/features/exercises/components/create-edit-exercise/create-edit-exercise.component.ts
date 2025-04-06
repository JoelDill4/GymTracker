import { Component, EventEmitter, Output, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Exercise, CreateExerciseDto } from '../../models/exercise.model';
import { ExerciseService } from '../../services/exercise.service';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { BodyPart } from '../../../bodyParts/models/body-part.model';
import { of } from 'rxjs';

@Component({
  selector: 'app-create-edit-exercise',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-edit-exercise.component.html',
  styleUrl: './create-edit-exercise.component.css'
})
export class CreateExerciseComponent implements OnInit {
  @Input() exerciseToEdit: Exercise | null = null;
  @Output() cancel = new EventEmitter<void>();
  @Output() exerciseCreated = new EventEmitter<Exercise>();
  @Output() exerciseUpdated = new EventEmitter<Exercise>();

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
  isEditing = false;

  constructor(
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) {}

  ngOnInit(): void {
    this.loadBodyParts();
    if (this.exerciseToEdit) {
      this.isEditing = true;
      this.exercise = { ...this.exerciseToEdit };
      this.selectedBodyPartId = this.exerciseToEdit.bodyPart.id;
    }
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
      const exerciseData: CreateExerciseDto = {
        name: this.exercise.name,
        description: this.exercise.description,
        fk_bodyPart: this.selectedBodyPartId
      };

      if (this.isEditing && this.exerciseToEdit?.id) {
        this.exerciseService.updateExercise(this.exerciseToEdit.id, exerciseData).subscribe({
          next: (updatedExercise) => {
            this.exerciseUpdated.emit(updatedExercise);
          },
          error: (error) => {
            console.error('Error updating exercise:', error);
          }
        });
      } else {
        this.exerciseService.createExercise(exerciseData).subscribe({
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
} 
