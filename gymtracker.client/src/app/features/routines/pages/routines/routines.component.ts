import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CreateRoutineComponent } from '../../components/create-edit-routine/create-edit-routine.component';
import { Routine } from '../../models/routine.model';
import { RoutineService } from '../../services/routine.service';

@Component({
  selector: 'app-routines',
  standalone: true,
  imports: [CommonModule, RouterModule, CreateRoutineComponent],
  templateUrl: './routines.component.html',
  styleUrls: ['./routines.component.css']
})
export class RoutinesComponent implements OnInit {
  routines: Routine[] = [];
  showCreateModal = false;
  routineToEdit?: Routine;

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

  openCreateModal(): void {
    this.routineToEdit = undefined;
    this.showCreateModal = true;
  }

  openEditModal(routine: Routine): void {
    this.routineToEdit = routine;
    this.showCreateModal = true;
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

  onRoutineCreated(routine: Routine): void {
    this.routines.push(routine);
    this.showCreateModal = false;
  }

  onRoutineUpdated(routine: Routine): void {
    const index = this.routines.findIndex(r => r.id === routine.id);
    if (index !== -1) {
      this.routines[index] = routine;
    }
    this.showCreateModal = false;
  }

  onModalClosed(): void {
    this.showCreateModal = false;
    this.routineToEdit = undefined;
  }
} 
