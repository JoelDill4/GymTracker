import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CreateExerciseComponent } from './create-edit-exercise.component';
import { ExerciseService } from '../../services/exercise.service';
import { Exercise } from '../../models/exercise.model';
import { BodyPartService } from '../../../bodyParts/services/body-part.service';
import { of, throwError } from 'rxjs';
import { BodyPart } from '../../../bodyParts/models/body-part.model';

describe('CreateExerciseComponent', () => {
  let component: CreateExerciseComponent;
  let fixture: ComponentFixture<CreateExerciseComponent>;
  let exerciseService: jasmine.SpyObj<ExerciseService>;
  let bodyPartService: jasmine.SpyObj<BodyPartService>;

  const mockBodyPart: BodyPart = {
    id: '1',
    name: 'Chest',
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };

  const mockExercise: Exercise = {
    id: '1',
    name: 'Bench Press',
    description: 'Chest exercise',
    bodyPart: mockBodyPart,
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };

  beforeEach(async () => {
    const exerciseServiceSpy = jasmine.createSpyObj('ExerciseService', ['createExercise', 'updateExercise']);
    const bodyPartServiceSpy = jasmine.createSpyObj('BodyPartService', ['getBodyParts']);

    bodyPartServiceSpy.getBodyParts.and.returnValue(of([mockBodyPart]));

    await TestBed.configureTestingModule({
      imports: [CreateExerciseComponent],
      providers: [
        { provide: ExerciseService, useValue: exerciseServiceSpy },
        { provide: BodyPartService, useValue: bodyPartServiceSpy }
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
    it('should initialize with empty exercise in create mode', () => {
      expect(component.exercise).toEqual({
        name: '',
        description: '',
        bodyPart: {
          id: '',
          name: '',
          createdAt: jasmine.any(Date),
          updatedAt: jasmine.any(Date),
          isDeleted: false
        }
      });
      expect(component.isEditing).toBeFalse();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should initialize with provided exercise in edit mode', () => {
      component.exerciseToEdit = mockExercise;
      component.ngOnInit();
      
      expect(component.exercise).toEqual({
        name: mockExercise.name,
        description: mockExercise.description,
        bodyPart: mockExercise.bodyPart
      });
      expect(component.selectedBodyPartId).toBe(mockExercise.bodyPart.id);
      expect(component.isEditing).toBeTrue();
    });

    it('should load body parts on init', () => {
      component.ngOnInit();
      expect(bodyPartService.getBodyParts).toHaveBeenCalled();
      expect(component.bodyParts).toEqual([mockBodyPart]);
    });
  });

  describe('Form Validation', () => {
    it('should not submit if name is empty', fakeAsync(() => {
      component.exercise.name = '';
      component.selectedBodyPartId = '1';
      const createdSpy = spyOn(component.created, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(exerciseService.createExercise).not.toHaveBeenCalled();
      expect(component.error).toBe('Name is required');
    }));

    it('should not submit if body part is not selected', fakeAsync(() => {
      component.exercise.name = 'Test Exercise';
      component.selectedBodyPartId = '';
      const createdSpy = spyOn(component.created, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(exerciseService.createExercise).not.toHaveBeenCalled();
      expect(component.error).toBe('Body part is required');
    }));
  });

  describe('Create Mode', () => {
    it('should create exercise and emit created event on successful submit', fakeAsync(() => {
      const newExercise: Exercise = {
        id: '2',
        name: 'New Exercise',
        description: 'Test Description',
        bodyPart: mockBodyPart,
        createdAt: new Date(),
        updatedAt: new Date(),
        isDeleted: false
      };

      exerciseService.createExercise.and.returnValue(of(newExercise));
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.exercise = {
        name: 'New Exercise',
        description: 'Test Description',
        bodyPart: mockBodyPart
      };
      component.selectedBodyPartId = '1';
      component.onSubmit();
      tick();

      expect(exerciseService.createExercise).toHaveBeenCalledWith({
        name: 'New Exercise',
        description: 'Test Description',
        bodyPart: {
          id: '1',
          name: 'Chest',
          createdAt: jasmine.any(Date),
          updatedAt: jasmine.any(Date),
          isDeleted: false
        }
      });
      expect(createdSpy).toHaveBeenCalledWith(newExercise);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when creating exercise fails', fakeAsync(() => {
      exerciseService.createExercise.and.returnValue(throwError(() => new Error('Failed to create')));

      component.exercise = {
        name: 'New Exercise',
        description: 'Test Description',
        bodyPart: {
          id: '1',
          name: 'Chest',
          createdAt: new Date(),
          updatedAt: new Date(),
          isDeleted: false
        }
      };
      component.selectedBodyPartId = '1';
      component.onSubmit();
      tick();

      expect(exerciseService.createExercise).toHaveBeenCalledWith({
        name: 'New Exercise',
        description: 'Test Description',
        bodyPart: {
          id: '1',
          name: 'Chest',
          createdAt: jasmine.any(Date),
          updatedAt: jasmine.any(Date),
          isDeleted: false
        }
      });
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to create exercise');
    }));
  });

  describe('Edit Mode', () => {
    beforeEach(() => {
      component.exerciseToEdit = mockExercise;
      component.ngOnInit();
    });

    it('should update exercise and emit updated event on successful submit', fakeAsync(() => {
      const updatedExercise = { ...mockExercise, name: 'Updated Exercise' };
      exerciseService.updateExercise.and.returnValue(of(updatedExercise));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.exercise.name = 'Updated Exercise';
      component.onSubmit();
      tick();

      expect(exerciseService.updateExercise).toHaveBeenCalledWith(
        mockExercise.id,
        {
          name: 'Updated Exercise',
          description: mockExercise.description,
          bodyPart: {
            id: '1',
            name: 'Chest',
            createdAt: jasmine.any(Date),
            updatedAt: jasmine.any(Date),
            isDeleted: false
          }
        }
      );
      expect(updatedSpy).toHaveBeenCalledWith(updatedExercise);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when updating exercise fails', fakeAsync(() => {
      exerciseService.updateExercise.and.returnValue(throwError(() => new Error('Update failed')));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.exercise.name = 'Updated Exercise';
      component.onSubmit();
      tick();

      expect(exerciseService.updateExercise).toHaveBeenCalledWith(
        mockExercise.id,
        {
          name: 'Updated Exercise',
          description: mockExercise.description,
          bodyPart: {
            id: '1',
            name: 'Chest',
            createdAt: jasmine.any(Date),
            updatedAt: jasmine.any(Date),
            isDeleted: false
          }
        }
      );
      expect(updatedSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to update exercise');
    }));
  });

  describe('Modal Actions', () => {
    it('should emit cancel event when close button is clicked', () => {
      const cancelSpy = spyOn(component.cancel, 'emit');
      const closeButton = fixture.nativeElement.querySelector('.btn-close');
      
      closeButton.click();
      
      expect(cancelSpy).toHaveBeenCalled();
    });

    it('should emit cancel event when cancel button is clicked', () => {
      const cancelSpy = spyOn(component.cancel, 'emit');
      const cancelButton = fixture.nativeElement.querySelector('.btn-outline-secondary');
      
      cancelButton.click();
      
      expect(cancelSpy).toHaveBeenCalled();
    });
  });
}); 
