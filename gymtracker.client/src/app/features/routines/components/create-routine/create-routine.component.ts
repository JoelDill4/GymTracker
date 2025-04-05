import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoutineService } from '../../services/routine.service';

@Component({
  selector: 'app-create-routine',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-routine.component.html',
  styleUrls: ['./create-routine.component.css']
})
export class CreateRoutineComponent {
  @Output() close = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  routine = {
    name: '',
    description: ''
  };

  constructor(private routineService: RoutineService) {}

  onSubmit(): void {
    this.routineService.createRoutine(this.routine).subscribe({
      next: () => {
        this.created.emit();
      },
      error: (error) => {
        console.error('Error creating routine:', error);
      }
    });
  }
} 