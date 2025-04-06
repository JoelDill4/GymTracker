import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CreateExerciseComponent } from './create-edit-exercise.component';
import { ExerciseService } from '../../services/exercise.service';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { Exercise } from '../../models/exercise.model';
import { BodyPart } from '../../../bodyParts/models/body-part.model';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('CreateExerciseComponent', () => {
  let component: CreateExerciseComponent;
  let fixture: ComponentFixture<CreateExerciseComponent>;
  let exerciseService: jasmine.SpyObj<ExerciseService>;
  let bodyPartService: jasmine.SpyObj<BodyPartService>;

  const mockBodyParts: BodyPart[] = [
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

  const mockExercise: Exercise = {
    id: '1',
    name: 'Test Exercise',
    description: 'Test Description',
    bodyPart: {
      id: '1',
      name: 'Chest'
    }
  };

  beforeEach(async () => {
    const exerciseSpy = jasmine.createSpyObj('ExerciseService', ['createExercise', 'updateExercise']);
    const bodyPartSpy = jasmine.createSpyObj('BodyPartService', ['getBodyParts']);
    bodyPartSpy.getBodyParts.and.returnValue(of(mockBodyParts));

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      providers: [
        { provide: ExerciseService, useValue: exerciseSpy },
        { provide: BodyPartService, useValue: bodyPartSpy }
      ]
    }).compileComponents();

    exerciseService = TestBed.inject(ExerciseService) as jasmine.SpyObj<ExerciseService>;
    bodyPartService = TestBed.inject(BodyPartService) as jasmine.SpyObj<BodyPartService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateExerciseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
  it('should initialize with empty exercise and load body parts', () => {
    expect(component.exercise).toEqual({
      name: '',
      description: '',
      bodyPart: {
        id: '',
        name: ''
      }
    });
    expect(component.selectedBodyPartId).toBe('');
      expect(component.isEditing).toBeFalse();
    expect(bodyPartService.getBodyParts).toHaveBeenCalled();
  });

    it('should initialize in edit mode when exerciseToEdit is provided', () => {
      component.exerciseToEdit = mockExercise;
      component.ngOnInit();

      expect(component.isEditing).toBeTrue();
      expect(component.exercise).toEqual(mockExercise);
      expect(component.selectedBodyPartId).toBe(mockExercise.bodyPart.id);
    });
  });

  describe('Body Parts', () => {
  it('should load body parts on init', fakeAsync(() => {
    component.ngOnInit();
    tick();

    expect(bodyPartService.getBodyParts).toHaveBeenCalled();
    expect(component.bodyParts).toEqual(mockBodyParts);
  }));

    it('should handle error when loading body parts fails', fakeAsync(() => {
      bodyPartService.getBodyParts.and.returnValue(throwError(() => new Error('Failed to load')));
      const consoleSpy = spyOn(console, 'error');

      component.ngOnInit();
      tick();

      expect(bodyPartService.getBodyParts).toHaveBeenCalled();
      expect(consoleSpy).toHaveBeenCalledWith('Error loading body parts:', jasmine.any(Error));
    }));
  });

  describe('Create Mode', () => {
  it('should create exercise and emit event on successful submit', fakeAsync(() => {
    const newExercise: Exercise = {
        id: '2',
      name: 'Bench Press',
      description: 'Chest exercise',
      bodyPart: {
        id: '1',
        name: 'Chest'
      }
    };

    component.exercise.name = 'Bench Press';
    component.exercise.description = 'Chest exercise';
    component.selectedBodyPartId = '1';

    exerciseService.createExercise.and.returnValue(of(newExercise));
      const createdSpy = spyOn(component.exerciseCreated, 'emit');

    component.onSubmit();
    tick();

    expect(exerciseService.createExercise).toHaveBeenCalledWith({
      name: 'Bench Press',
      description: 'Chest exercise',
      fk_bodyPart: '1'
    });
      expect(createdSpy).toHaveBeenCalledWith(newExercise);
  }));

  it('should not submit if name or body part is missing', fakeAsync(() => {
    component.exercise.name = '';
    component.selectedBodyPartId = '';

    component.onSubmit();
    tick();

    expect(exerciseService.createExercise).not.toHaveBeenCalled();
  }));

  it('should handle error when creating exercise fails', fakeAsync(() => {
    component.exercise.name = 'Bench Press';
    component.exercise.description = 'Chest exercise';
    component.selectedBodyPartId = '1';

    exerciseService.createExercise.and.returnValue(throwError(() => new Error('Failed to create')));
      const consoleSpy = spyOn(console, 'error');
      const createdSpy = spyOn(component.exerciseCreated, 'emit');

    component.onSubmit();
    tick();

    expect(exerciseService.createExercise).toHaveBeenCalled();
      expect(createdSpy).not.toHaveBeenCalled();
      expect(consoleSpy).toHaveBeenCalledWith('Error creating exercise:', jasmine.any(Error));
  }));
  });

  describe('Edit Mode', () => {
    beforeEach(() => {
    component.exerciseToEdit = mockExercise;
    component.ngOnInit();
    });

    it('should update exercise and emit event on successful submit', fakeAsync(() => {
      const updatedExercise: Exercise = {
        ...mockExercise,
        name: 'Updated Exercise',
        description: 'Updated Description'
      };

      component.exercise.name = 'Updated Exercise';
      component.exercise.description = 'Updated Description';
      component.selectedBodyPartId = '1';

      exerciseService.updateExercise.and.returnValue(of(updatedExercise));
      const updatedSpy = spyOn(component.exerciseUpdated, 'emit');

      component.onSubmit();
      tick();

      expect(exerciseService.updateExercise).toHaveBeenCalledWith(
        mockExercise.id!,
        {
          name: 'Updated Exercise',
          description: 'Updated Description',
          fk_bodyPart: '1'
        }
      );
      expect(updatedSpy).toHaveBeenCalledWith(updatedExercise);
    }));

    it('should handle error when updating exercise fails', fakeAsync(() => {
      component.exercise.name = 'Updated Exercise';
      component.selectedBodyPartId = '1';

      exerciseService.updateExercise.and.returnValue(throwError(() => new Error('Failed to update')));
      const consoleSpy = spyOn(console, 'error');
      const updatedSpy = spyOn(component.exerciseUpdated, 'emit');

      component.onSubmit();
      tick();

      expect(exerciseService.updateExercise).toHaveBeenCalled();
      expect(updatedSpy).not.toHaveBeenCalled();
      expect(consoleSpy).toHaveBeenCalledWith('Error updating exercise:', jasmine.any(Error));
    }));
  });

  describe('Common Actions', () => {
    it('should emit cancel event when onCancel is called', () => {
      const cancelSpy = spyOn(component.cancel, 'emit');
      component.onCancel();
      expect(cancelSpy).toHaveBeenCalled();
    });
  });
}); 
