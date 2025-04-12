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

  describe('Creation Mode', () => {
    it('should initialize with empty routine', () => {
      expect(component.routine).toEqual({
        id: '',
        name: '',
        description: '',
        createdAt: jasmine.any(Date),
        updatedAt: jasmine.any(Date),
        isDeleted: false
      });
      expect(component.isEditing).toBeFalse();
    });

    it('should create routine and emit created event', fakeAsync(() => {
      const newRoutine = { ...mockRoutine, id: '2' };
      routineService.createRoutine.and.returnValue(of(newRoutine));
      const createdSpy = spyOn(component.created, 'emit');
      const closeSpy = spyOn(component.close, 'emit');

      component.routine.name = 'New Routine';
      component.routine.description = 'New Description';
      component.onSubmit();
      tick();

      expect(routineService.createRoutine).toHaveBeenCalledWith(component.routine);
      expect(createdSpy).toHaveBeenCalledWith(newRoutine);
      expect(closeSpy).toHaveBeenCalled();
    }));

    it('should handle creation error', fakeAsync(() => {
      routineService.createRoutine.and.returnValue(throwError(() => new Error('Creation failed')));
      const createdSpy = spyOn(component.created, 'emit');
      const closeSpy = spyOn(component.close, 'emit');

      component.routine.name = 'New Routine';
      component.onSubmit();
      tick();

      expect(routineService.createRoutine).toHaveBeenCalledWith(component.routine);
      expect(createdSpy).not.toHaveBeenCalled();
      expect(closeSpy).not.toHaveBeenCalled();
      expect(component.error).toBe('Failed to create routine');
    }));
  });

  describe('Edit Mode', () => {
    beforeEach(() => {
      component.routineToEdit = mockRoutine;
      component.ngOnInit();
    });

    it('should initialize with routine to edit', () => {
      expect(component.routine).toEqual(mockRoutine);
      expect(component.isEditing).toBeTrue();
    });

    it('should update routine and emit updated event', fakeAsync(() => {
      const updatedRoutine = { ...mockRoutine, name: 'Updated Routine' };
      routineService.updateRoutine.and.returnValue(of(updatedRoutine));
      const updatedSpy = spyOn(component.updated, 'emit');
      const closeSpy = spyOn(component.close, 'emit');

      component.routine.name = 'Updated Routine';
      component.onSubmit();
      tick();

      expect(routineService.updateRoutine).toHaveBeenCalledWith(mockRoutine.id, component.routine);
      expect(updatedSpy).toHaveBeenCalledWith(updatedRoutine);
      expect(closeSpy).toHaveBeenCalled();
    }));

    it('should handle update error', fakeAsync(() => {
      routineService.updateRoutine.and.returnValue(throwError(() => new Error('Update failed')));
      const updatedSpy = spyOn(component.updated, 'emit');
      const closeSpy = spyOn(component.close, 'emit');

      component.routine.name = 'Updated Routine';
      component.onSubmit();
      tick();

      expect(routineService.updateRoutine).toHaveBeenCalledWith(mockRoutine.id, component.routine);
      expect(updatedSpy).not.toHaveBeenCalled();
      expect(closeSpy).not.toHaveBeenCalled();
      expect(component.error).toBe('Failed to update routine');
    }));
  });

  describe('Modal Actions', () => {
    it('should emit close event when close button is clicked', () => {
      const closeSpy = spyOn(component.close, 'emit');
      const closeButton = fixture.nativeElement.querySelector('.btn-close');
      
      closeButton.click();
      
      expect(closeSpy).toHaveBeenCalled();
    });

    it('should emit close event when cancel button is clicked', () => {
      const closeSpy = spyOn(component.close, 'emit');
      const cancelButton = fixture.nativeElement.querySelector('.btn-outline-secondary');
      
      cancelButton.click();
      
      expect(closeSpy).toHaveBeenCalled();
    });
  });
}); 
