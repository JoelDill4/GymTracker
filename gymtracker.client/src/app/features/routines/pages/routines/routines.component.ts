import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CreateRoutineComponent } from '../../components/create-edit-routine/create-edit-routine.component';
import { Routine } from '../../models/routine.model';
import { RoutineService } from '../../services/routine.service';
import { NewButtonComponent } from '../../../../shared/components/new-button/new-button.component';
import { ListCardComponent } from '../../../../shared/components/list-card/list-card.component';
import { EditButtonComponent } from '../../../../shared/components/edit-button/edit-button.component';
import { DeleteButtonComponent } from '../../../../shared/components/delete-button/delete-button.component';

@Component({
  selector: 'app-routines',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    CreateRoutineComponent, 
    NewButtonComponent, 
    ListCardComponent,
    EditButtonComponent,
    DeleteButtonComponent
  ],
  templateUrl: './routines.component.html',
  styleUrls: ['./routines.component.css']
})
export class RoutinesComponent implements OnInit {
  routines: Routine[] = [];
  showCreateModal = false;
  routineToEdit?: Routine;
  loading = false;
  error: string | null = null;

  constructor(private routineService: RoutineService) {}

  ngOnInit(): void {
    this.loadRoutines();
  }

  loadRoutines(): void {
    this.loading = true;
    this.error = null;
    this.routineService.getRoutines().subscribe({
      next: (routines) => {
        this.routines = routines;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading routines:', error);
        this.error = 'Failed to load routines';
        this.loading = false;
      }
    });
  }

  openCreateModal(): void {
    this.showCreateModal = true;
  }

  openEditModal(routine: Routine): void {
    this.routineToEdit = routine;
    this.showCreateModal = true;
  }

  onModalClosed(): void {
    this.showCreateModal = false;
    this.routineToEdit = undefined;
  }

  onRoutineCreated(routine: Routine): void {
    this.routines.push(routine);
    this.showCreateModal = false;
  }

  onRoutineUpdated(updatedRoutine: Routine): void {
    const index = this.routines.findIndex(r => r.id === updatedRoutine.id);
    if (index !== -1) {
      this.routines[index] = updatedRoutine;
    }
    this.showCreateModal = false;
    this.routineToEdit = undefined;
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

  // Helper methods for ListCardComponent
  getRoutineTitle = (routine: Routine): string => routine.name;
  getRoutineDescription = (routine: Routine): string | undefined => routine.description;
  getRoutineRouterLink = (routine: Routine): any[] => ['/routines', routine.id];
  onRoutineClick = (routine: Routine): void => {
    // Handle routine click if needed
  };
} 
