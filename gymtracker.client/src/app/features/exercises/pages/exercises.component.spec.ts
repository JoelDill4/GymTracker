import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ExercisesComponent } from './exercises.component';
import { ExerciseService } from '../services/exercise.service';
import { Exercise } from '../models/exercise.model';
import { CommonModule } from '@angular/common';
import { CreateExerciseComponent } from '../components/create-edit-exercise/create-edit-exercise.component';
import { of, throwError } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BodyPartService } from '../../bodyParts/services/body-part.service';
import { ActivatedRoute } from '@angular/router';

describe('ExercisesComponent', () => {
  let component: ExercisesComponent;
  let fixture: ComponentFixture<ExercisesComponent>;
  let exerciseService: jasmine.SpyObj<ExerciseService>;
  let bodyPartService: jasmine.SpyObj<BodyPartService>;

  const mockExercises: Exercise[] = [
    {
      id: '1',
      name: 'Bench Press',
      description: 'Chest exercise',
      bodyPart: {
        id: '1',
        name: 'Chest',
        createdAt: new Date(),
        updatedAt: new Date(),
        isDeleted: false
      },
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false
    },
    {
      id: '2',
      name: 'Squat',
      description: 'Leg exercise',
      bodyPart: {
        id: '2',
        name: 'Legs',
        createdAt: new Date(),
        updatedAt: new Date(),
        isDeleted: false
      },
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false
    }
  ];

  const mockBodyParts = [
    {
      id: '1',
      name: 'Chest',
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false
    },
    {
      id: '2',
      name: 'Legs',
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false
    }
  ];

  beforeEach(async () => {
    const exerciseSpy = jasmine.createSpyObj('ExerciseService', [
      'getExercises',
      'searchExercisesByName',
      'getExercisesByBodyPart'
    ]);
    exerciseSpy.getExercises.and.returnValue(of(mockExercises));
    exerciseSpy.searchExercisesByName.and.returnValue(of(mockExercises));
    exerciseSpy.getExercisesByBodyPart.and.returnValue(of(mockExercises));

    const bodyPartSpy = jasmine.createSpyObj('BodyPartService', ['getBodyParts']);
    bodyPartSpy.getBodyParts.and.returnValue(of(mockBodyParts));

    await TestBed.configureTestingModule({
      imports: [
        CommonModule,
        CreateExerciseComponent,
        HttpClientTestingModule
      ],
      providers: [
        { provide: ExerciseService, useValue: exerciseSpy },
        { provide: BodyPartService, useValue: bodyPartSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({})
          }
        }
      ]
    }).compileComponents();

    exerciseService = TestBed.inject(ExerciseService) as jasmine.SpyObj<ExerciseService>;
    bodyPartService = TestBed.inject(BodyPartService) as jasmine.SpyObj<BodyPartService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExercisesComponent);
    component = fixture.componentInstance;
    component.exercises = [...mockExercises];
    component.filteredExercises = [...mockExercises];
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty exercises array and loading false', () => {
    const newComponent = new ExercisesComponent(
      exerciseService,
      bodyPartService
    );
    expect(newComponent.exercises).toEqual([]);
    expect(newComponent.loading).toBeFalse();
    expect(newComponent.error).toBeNull();
    expect(newComponent.showCreateModal).toBeFalse();
  });

  it('should load exercises on init', fakeAsync(() => {
    component.ngOnInit();
    tick();

    expect(exerciseService.getExercises).toHaveBeenCalled();
    expect(component.exercises).toEqual(mockExercises);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should handle error when loading exercises fails', fakeAsync(() => {
    exerciseService.getExercises.and.returnValue(throwError(() => new Error('Failed to load')));
    component.exercises = [];
    component.filteredExercises = [];

    component.ngOnInit();
    tick();

    expect(exerciseService.getExercises).toHaveBeenCalled();
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Failed to load exercises');
  }));

  it('should search exercises by name', fakeAsync(() => {
    component.onSearch('Bench');
    tick();

    expect(component.currentSearchTerm).toBe('Bench');
    expect(component.filteredExercises).toEqual(mockExercises.filter(e => e.name.toLowerCase().includes('bench')));
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should handle error when searching exercises fails', fakeAsync(() => {
    exerciseService.searchExercisesByName.and.returnValue(throwError(() => new Error('Failed to search')));
    component.exercises = [];
    component.filteredExercises = [];

    component.onSearch('Bench');
    tick();

    expect(component.currentSearchTerm).toBe('Bench');
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should clear search and reload all exercises', fakeAsync(() => {
    component.onSearch('Bench');
    component.onSearch('');
    tick();

    expect(component.currentSearchTerm).toBe('');
    expect(component.filteredExercises).toEqual(mockExercises);
  }));

  it('should filter exercises by body part', fakeAsync(() => {
    component.onFilter('1');
    tick();

    expect(component.currentBodyPartId).toBe('1');
    expect(component.filteredExercises).toEqual(mockExercises.filter(e => e.bodyPart.id === '1'));
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should handle error when filtering by body part fails', fakeAsync(() => {
    exerciseService.getExercisesByBodyPart.and.returnValue(throwError(() => new Error('Failed to filter')));
    component.exercises = [];
    component.filteredExercises = [];

    component.onFilter('1');
    tick();

    expect(component.currentBodyPartId).toBe('1');
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should clear body part filter and reload all exercises', fakeAsync(() => {
    component.onFilter('1');
    component.onFilter(null);
    tick();

    expect(component.currentBodyPartId).toBeNull();
    expect(component.filteredExercises).toEqual(mockExercises);
  }));
});
