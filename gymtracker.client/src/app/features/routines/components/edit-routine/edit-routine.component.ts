import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoutineService } from '../../services/routine.service';
import { Routine } from '../../models/routine.model';

@Component({
  selector: 'app-edit-routine',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-routine.component.html',
  styleUrls: ['./edit-routine.component.css']
})
export class EditRoutineComponent {
  @Input() routine!: Routine;
  @Output() close = new EventEmitter<void>();
  @Output() updated = new EventEmitter<void>();

  constructor(private routineService: RoutineService) {}

  onSubmit(): void {
    this.routineService.updateRoutine(this.routine.id, {
      name: this.routine.name,
      description: this.routine.description
    }).subscribe({
      next: () => {
        this.updated.emit();
      },
      error: (error) => {
        console.error('Error updating routine:', error);
      }
    });
  }
} 