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
      imports: [FormsModule, CreateExerciseComponent],
      providers: [
        { provide: ExerciseService, useValue: exerciseSpy },
        { provide: BodyPartService, useValue: bodyPartSpy }
      ]
    }).compileComponents();

    exerciseService = TestBed.inject(ExerciseService) as jasmine.SpyObj<ExerciseService>;
    bodyPartService = TestBed.inject(BodyPartService) as jasmine.SpyObj<BodyPartService>;
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
          name: ''
        }
      });
      expect(component.selectedBodyPartId).toBe('');
      expect(component.isEditing).toBeFalse();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should initialize with provided exercise in edit mode', () => {
      component.exerciseToEdit = mockExercise;
      component.ngOnInit();
      
      expect(component.exercise).toEqual(mockExercise);
      expect(component.selectedBodyPartId).toBe(mockExercise.bodyPart.id);
      expect(component.isEditing).toBeTrue();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should load body parts on init', fakeAsync(() => {
      component.ngOnInit();
      tick();

      expect(bodyPartService.getBodyParts).toHaveBeenCalled();
      expect(component.bodyParts).toEqual(mockBodyParts);
      expect(component.error).toBe('');
    }));

    it('should handle error when loading body parts fails', fakeAsync(() => {
      bodyPartService.getBodyParts.and.returnValue(throwError(() => new Error('Failed to load')));
      
      component.ngOnInit();
      tick();

      expect(bodyPartService.getBodyParts).toHaveBeenCalled();
      expect(component.error).toBe('Failed to load body parts');
    }));
  });

  describe('Form Validation', () => {
    it('should not submit if name is empty', fakeAsync(() => {
      component.exercise.name = '';
      component.selectedBodyPartId = '1';
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(exerciseService.createExercise).not.toHaveBeenCalled();
      expect(component.error).toBe('Name is required');
    }));

    it('should not submit if body part is not selected', fakeAsync(() => {
      component.exercise.name = 'Test Exercise';
      component.selectedBodyPartId = '';
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(exerciseService.createExercise).not.toHaveBeenCalled();
      expect(component.error).toBe('Body part is required');
    }));
  });

  describe('Create Mode', () => {
    it('should create exercise and emit created event on successful submit', fakeAsync(() => {
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
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.onSubmit();
      tick();

      expect(exerciseService.createExercise).toHaveBeenCalledWith({
        name: 'Bench Press',
        description: 'Chest exercise',
        fk_bodyPart: '1'
      });
      expect(createdSpy).toHaveBeenCalledWith(newExercise);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when creating exercise fails', fakeAsync(() => {
      component.exercise.name = 'Bench Press';
      component.exercise.description = 'Chest exercise';
      component.selectedBodyPartId = '1';

      exerciseService.createExercise.and.returnValue(throwError(() => new Error('Failed to create')));
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.onSubmit();
      tick();

      expect(exerciseService.createExercise).toHaveBeenCalled();
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
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
      const updatedExercise: Exercise = {
        ...mockExercise,
        name: 'Updated Exercise',
        description: 'Updated Description'
      };

      component.exercise.name = 'Updated Exercise';
      component.exercise.description = 'Updated Description';
      component.selectedBodyPartId = '1';

      exerciseService.updateExercise.and.returnValue(of(updatedExercise));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

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
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when updating exercise fails', fakeAsync(() => {
      component.exercise.name = 'Updated Exercise';
      component.selectedBodyPartId = '1';

      exerciseService.updateExercise.and.returnValue(throwError(() => new Error('Failed to update')));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.onSubmit();
      tick();

      expect(exerciseService.updateExercise).toHaveBeenCalled();
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
