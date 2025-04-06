import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routine } from '../../models/routine.model';
import { RoutineService } from '../../services/routine.service';

@Component({
  selector: 'app-create-edit-routine',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-edit-routine.component.html',
  styleUrls: ['./create-edit-routine.component.css']
})
export class CreateRoutineComponent implements OnInit {
  @Input() routineToEdit?: Routine;
  @Output() close = new EventEmitter<void>();
  @Output() created = new EventEmitter<Routine>();
  @Output() updated = new EventEmitter<Routine>();

  routine: Routine = {
    id: '',
    name: '',
    description: '',
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };
  loading = false;
  error = '';
  isEditing = false;

  constructor(private routineService: RoutineService) {}

  ngOnInit(): void {
    if (this.routineToEdit) {
      this.isEditing = true;
      this.routine = { ...this.routineToEdit };
    }
  }

  onSubmit(): void {
    if (this.isEditing) {
      this.updateRoutine();
    } else {
      this.createRoutine();
    }
  }

  private createRoutine(): void {
    this.loading = true;
    this.error = '';
    this.routineService.createRoutine(this.routine).subscribe({
      next: (routine) => {
        this.loading = false;
        this.created.emit(routine);
        this.close.emit();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Failed to create routine';
      }
    });
  }

  private updateRoutine(): void {
    this.loading = true;
    this.error = '';
    this.routineService.updateRoutine(this.routine.id, this.routine).subscribe({
      next: (routine) => {
        this.loading = false;
        this.updated.emit(routine);
        this.close.emit();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Failed to update routine';
      }
    });
  }

  onCancel(): void {
    this.close.emit();
  }
} 