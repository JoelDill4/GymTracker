import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RoutineService } from '../../services/routine.service';
import { Routine } from '../../models/routine.model';
import { CreateRoutineComponent } from '../../components/create-routine/create-routine.component';
import { EditRoutineComponent } from '../../components/edit-routine/edit-routine.component';

@Component({
  selector: 'app-routines',
  templateUrl: './routine.component.html',
  styleUrl: './routine.component.css',
  standalone: true,
  imports: [CommonModule, RouterModule, CreateRoutineComponent, EditRoutineComponent]
})

export class RoutinesComponent implements OnInit {
  routines: Routine[] = [];
  showCreateModal = false;
  showEditModal = false;
  selectedRoutine!: Routine;

  constructor(private routineService: RoutineService) {}

  ngOnInit(): void {
    this.loadRoutines();
  }

  loadRoutines(): void {
    this.routineService.getRoutines().subscribe({
      next: (routines) => {
        this.routines = routines;
      },
      error: (error) => {
        console.error('Error loading routines:', error);
      }
    });
  }

  editRoutine(routine: Routine): void {
    this.selectedRoutine = { ...routine };
    this.showEditModal = true;
  }

  deleteRoutine(id: string): void {
    if (confirm('Are you sure you want to delete this routine?')) {
      this.routineService.deleteRoutine(id).subscribe({
        next: () => {
          this.routines = this.routines.filter(r => r.id !== id);
        },
        error: (error) => {
          console.error('Error deleting routine:', error);
        }
      });
    }
  }

  onRoutineCreated(): void {
    this.showCreateModal = false;
    this.loadRoutines();
  }

  onRoutineUpdated(): void {
    this.showEditModal = false;
    this.loadRoutines();
  }
} 
