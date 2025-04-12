import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BaseModalComponent } from '../../../../shared/components/base-modal/base-modal.component';
import { FormFieldComponent } from '../../../../shared/components/form-field/form-field.component';
import { ModalFooterComponent } from '../../../../shared/components/modal-footer/modal-footer.component';
import { BodyPart } from '../../../bodyParts/models/body-part.model';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { CreateExerciseDto, Exercise } from '../../models/exercise.model';
import { ExerciseService } from '../../services/exercise.service';

@Component({
  selector: 'app-create-edit-exercise',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    BaseModalComponent,
    FormFieldComponent,
    ModalFooterComponent
  ],
  templateUrl: './create-edit-exercise.component.html'
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
