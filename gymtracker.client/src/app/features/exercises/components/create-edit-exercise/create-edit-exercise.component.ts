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
  @Output() created = new EventEmitter<Exercise>();
  @Output() updated = new EventEmitter<Exercise>();

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
  loading = false;
  error = '';

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
      error: (err) => {
        this.error = err.error?.message || 'Failed to load body parts';
      }
    });
  }

  onSubmit(): void {
    if (!this.exercise.name) {
      this.error = 'Name is required';
      return;
    }

    if (!this.selectedBodyPartId) {
      this.error = 'Body part is required';
      return;
    }

    const exerciseData: CreateExerciseDto = {
      name: this.exercise.name,
      description: this.exercise.description,
      fk_bodyPart: this.selectedBodyPartId
    };

    this.loading = true;
    this.error = '';

    if (this.isEditing && this.exerciseToEdit?.id) {
      this.exerciseService.updateExercise(this.exerciseToEdit.id, exerciseData).subscribe({
        next: (updatedExercise) => {
          this.loading = false;
          this.updated.emit(updatedExercise);
          this.cancel.emit();
        },
        error: (err) => {
          this.loading = false;
          this.error = err.error?.message || 'Failed to update exercise';
        }
      });
    } else {
      this.exerciseService.createExercise(exerciseData).subscribe({
        next: (createdExercise) => {
          this.loading = false;
          this.created.emit(createdExercise);
          this.cancel.emit();
        },
        error: (err) => {
          this.loading = false;
          this.error = err.error?.message || 'Failed to create exercise';
        }
      });
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
} 
