import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Exercise } from '../models/exercise.model';
import { ExerciseService } from '../services/exercise.service';
import { BodyPartService } from '../../bodyParts/services/body-part.service';
import { BodyPart } from '../../bodyParts/models/body-part.model';
import { CreateExerciseComponent } from '../components/create-edit-exercise/create-edit-exercise.component';
import { NewButtonComponent } from '../../../shared/components/new-button/new-button.component';
import { ListCardComponent, FilterOption } from '../../../shared/components/list-card/list-card.component';
import { EditButtonComponent } from '../../../shared/components/edit-button/edit-button.component';
import { DeleteButtonComponent } from '../../../shared/components/delete-button/delete-button.component';
import { ProgressionChartComponent } from './progression-chart/progression-chart.component';

@Component({
  selector: 'app-exercises',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    FormsModule, 
    CreateExerciseComponent, 
    NewButtonComponent, 
    ListCardComponent,
    EditButtonComponent,
    DeleteButtonComponent,
    ProgressionChartComponent
  ],
  templateUrl: './exercises.component.html',
  styleUrl: './exercises.component.css'
})
export class ExercisesComponent implements OnInit {
  @Input() selectedSegment: string = 'My exercises';
  exercises: Exercise[] = [];
  filteredExercises: Exercise[] = [];
  bodyParts: BodyPart[] = [];
  loading = false;
  error: string | null = null;
  showCreateModal = false;
  selectedExercise: Exercise | null = null;
  currentSearchTerm: string = '';
  currentBodyPartId: string | null = null;

  constructor(
    private exerciseService: ExerciseService,
    private bodyPartService: BodyPartService
  ) {
  }

  ngOnInit(): void {
    this.loadExercises();
    this.loadBodyParts();
  }

  loadExercises(): void {
    this.loading = true;
    this.error = null;
    this.exerciseService.getExercises().subscribe({
      next: (data) => {
        this.exercises = data;
        this.filteredExercises = data;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load exercises';
        this.loading = false;
      }
    });
  }

  private applyFilters(): void {
    this.filteredExercises = this.exercises.filter(exercise => {
      const matchesSearch = !this.currentSearchTerm || 
        exercise.name.toLowerCase().includes(this.currentSearchTerm.toLowerCase());
      const matchesBodyPart = !this.currentBodyPartId || 
        exercise.bodyPart.id === this.currentBodyPartId;
      return matchesSearch && matchesBodyPart;
    });
  }

  loadBodyParts(): void {
    this.bodyPartService.getBodyParts().subscribe({
      next: (data) => {
        this.bodyParts = data;
      },
      error: (error) => {
        console.error('Error loading body parts:', error);
      }
    });
  }

  onSearch(searchTerm: string): void {
    this.currentSearchTerm = searchTerm;
    this.applyFilters();
  }

  onFilter(bodyPartId: string | null): void {
    this.currentBodyPartId = bodyPartId;
    this.applyFilters();
  }

  onCreateExercise(): void {
    this.showCreateModal = true;
  }

  onCancelCreate(): void {
    this.showCreateModal = false;
    this.selectedExercise = null;
  }

  onExerciseCreated(exercise: Exercise): void {
    this.exercises.push(exercise);
    this.showCreateModal = false;
  }

  onEditExercise(exercise: Exercise): void {
    this.selectedExercise = exercise;
    this.showCreateModal = true;
  }

  onExerciseUpdated(updatedExercise: Exercise): void {
    const index = this.exercises.findIndex(e => e.id === updatedExercise.id);
    if (index !== -1) {
      this.exercises[index] = updatedExercise;
    }
    this.showCreateModal = false;
    this.selectedExercise = null;
  }

  onDeleteExercise(id: string): void {
    if (confirm('Are you sure you want to delete this exercise?')) {
      this.loading = true;
      this.error = null;
      this.exerciseService.deleteExercise(id).subscribe({
        next: () => {
          console.log('Exercise deleted successfully');
          this.exercises = this.exercises.filter(e => e.id !== id);
          this.loading = false;
        },
        error: (error) => {
          console.error('Error deleting exercise:', error);
          this.error = 'Failed to delete exercise';
          this.loading = false;
        }
      });
    }
  }

  getExerciseTitle = (exercise: Exercise): string => exercise.name;
  getExerciseDescription = (exercise: Exercise): string | undefined => exercise.description;
  getExerciseBadge = (exercise: Exercise): { text: string; icon?: string; routerLink?: any[] } | undefined => ({
    text: exercise.bodyPart.name,
    icon: 'bi-person-arms-up'
  });
  getExerciseRouterLink = (exercise: Exercise): any[] => [];
  onExerciseClick = (exercise: Exercise): void => {
  };
} 
