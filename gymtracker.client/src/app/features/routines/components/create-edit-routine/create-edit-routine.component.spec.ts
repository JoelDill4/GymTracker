import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CreateRoutineComponent } from './create-edit-routine.component';
import { RoutineService } from '../../services/routine.service';
import { of, throwError } from 'rxjs';
import { Routine } from '../../models/routine.model';

describe('CreateRoutineComponent', () => {
  let component: CreateRoutineComponent;
  let fixture: ComponentFixture<CreateRoutineComponent>;
  let routineService: jasmine.SpyObj<RoutineService>;

  const mockRoutine: Routine = {
    id: '1',
    name: 'Test Routine',
    description: 'Test Description',
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };

  beforeEach(async () => {
    const routineServiceSpy = jasmine.createSpyObj('RoutineService', ['createRoutine', 'updateRoutine']);

    await TestBed.configureTestingModule({
      imports: [CreateRoutineComponent],
      providers: [
        { provide: RoutineService, useValue: routineServiceSpy }
      ]
    }).compileComponents();

    routineService = TestBed.inject(RoutineService) as jasmine.SpyObj<RoutineService>;
    fixture = TestBed.createComponent(CreateRoutineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
    it('should initialize with empty routine in create mode', () => {
      expect(component.routine).toEqual({
        id: '',
        name: '',
        description: '',
        createdAt: jasmine.any(Date),
        updatedAt: jasmine.any(Date),
        isDeleted: false
      });
      expect(component.isEditing).toBeFalse();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should initialize with provided routine in edit mode', () => {
      component.routineToEdit = mockRoutine;
      component.ngOnInit();
      
      expect(component.routine).toEqual(mockRoutine);
      expect(component.isEditing).toBeTrue();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });
  });

  describe('Form Validation', () => {
    it('should not submit if name is empty', fakeAsync(() => {
      component.routine.name = '';
      component.routine.description = 'Test Description';
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(routineService.createRoutine).not.toHaveBeenCalled();
      expect(component.error).toBe('Name is required');
    }));
  });

  describe('Create Mode', () => {
    it('should create routine and emit created event on successful submit', fakeAsync(() => {
      const newRoutine = { ...mockRoutine, id: '2' };
      routineService.createRoutine.and.returnValue(of(newRoutine));
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.routine.name = 'New Routine';
      component.routine.description = 'New Description';
      component.onSubmit();
      tick();

      expect(routineService.createRoutine).toHaveBeenCalledWith(component.routine);
      expect(createdSpy).toHaveBeenCalledWith(newRoutine);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when creating routine fails', fakeAsync(() => {
      routineService.createRoutine.and.returnValue(throwError(() => new Error('Creation failed')));
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.routine.name = 'New Routine';
      component.onSubmit();
      tick();

      expect(routineService.createRoutine).toHaveBeenCalledWith(component.routine);
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to create routine');
    }));
  });

  describe('Edit Mode', () => {
    beforeEach(() => {
      component.routineToEdit = mockRoutine;
      component.ngOnInit();
    });

    it('should update routine and emit updated event on successful submit', fakeAsync(() => {
      const updatedRoutine = { ...mockRoutine, name: 'Updated Routine' };
      routineService.updateRoutine.and.returnValue(of(updatedRoutine));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.routine.name = 'Updated Routine';
      component.onSubmit();
      tick();

      expect(routineService.updateRoutine).toHaveBeenCalledWith(mockRoutine.id, component.routine);
      expect(updatedSpy).toHaveBeenCalledWith(updatedRoutine);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when updating routine fails', fakeAsync(() => {
      routineService.updateRoutine.and.returnValue(throwError(() => new Error('Update failed')));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.routine.name = 'Updated Routine';
      component.onSubmit();
      tick();

      expect(routineService.updateRoutine).toHaveBeenCalledWith(mockRoutine.id, component.routine);
      expect(updatedSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to update routine');
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
