import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ExercisesComponent } from './exercises.component';
import { ExerciseService } from '../services/exercise.service';
import { Exercise } from '../models/exercise.model';
import { CommonModule } from '@angular/common';
import { CreateExerciseComponent } from '../components/create-edit-exercise/create-edit-exercise.component';
import { of, throwError } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BodyPartService } from '../../bodyParts/services/body-part.service';

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
        name: 'Chest'
      }
    },
    {
      id: '2',
      name: 'Squat',
      description: 'Leg exercise',
      bodyPart: {
        id: '2',
        name: 'Legs'
      }
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
        { provide: BodyPartService, useValue: bodyPartSpy }
      ]
    }).compileComponents();

    exerciseService = TestBed.inject(ExerciseService) as jasmine.SpyObj<ExerciseService>;
    bodyPartService = TestBed.inject(BodyPartService) as jasmine.SpyObj<BodyPartService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExercisesComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty exercises array and loading false', () => {
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
    expect(component.showCreateModal).toBeFalse();
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

    component.ngOnInit();
    tick();

    expect(exerciseService.getExercises).toHaveBeenCalled();
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Failed to load exercises');
  }));

  it('should search exercises by name', fakeAsync(() => {
    component.searchTerm = 'Bench';
    component.onSearch();
    tick();

    expect(exerciseService.searchExercisesByName).toHaveBeenCalledWith('Bench');
    expect(component.exercises).toEqual(mockExercises);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should handle error when searching exercises fails', fakeAsync(() => {
    exerciseService.searchExercisesByName.and.returnValue(throwError(() => new Error('Failed to search')));

    component.searchTerm = 'Bench';
    component.onSearch();
    tick();

    expect(exerciseService.searchExercisesByName).toHaveBeenCalledWith('Bench');
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Failed to search exercises');
  }));

  it('should clear search and reload all exercises', fakeAsync(() => {
    component.searchTerm = 'Bench';
    component.clearSearch();
    tick();

    expect(component.searchTerm).toBe('');
    expect(exerciseService.getExercises).toHaveBeenCalled();
  }));

  it('should filter exercises by body part', fakeAsync(() => {
    component.selectedBodyPartId = '1';
    component.onBodyPartChange();
    tick();

    expect(exerciseService.getExercisesByBodyPart).toHaveBeenCalledWith('1');
    expect(component.exercises).toEqual(mockExercises);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should handle error when filtering by body part fails', fakeAsync(() => {
    exerciseService.getExercisesByBodyPart.and.returnValue(throwError(() => new Error('Failed to filter')));

    component.selectedBodyPartId = '1';
    component.onBodyPartChange();
    tick();

    expect(exerciseService.getExercisesByBodyPart).toHaveBeenCalledWith('1');
    expect(component.exercises).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Failed to filter exercises by body part');
  }));

  it('should clear body part filter and reload all exercises', fakeAsync(() => {
    component.selectedBodyPartId = '1';
    component.clearBodyPartFilter();
    tick();

    expect(component.selectedBodyPartId).toBeNull();
    expect(exerciseService.getExercises).toHaveBeenCalled();
  }));
});
