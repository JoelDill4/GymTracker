import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RoutinesComponent } from './routines.component';
import { RoutineService } from '../../services/routine.service';
import { of, throwError } from 'rxjs';
import { Routine } from '../../models/routine.model';
import { CreateRoutineComponent } from '../../components/create-edit-routine/create-edit-routine.component';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

describe('RoutinesComponent', () => {
  let component: RoutinesComponent;
  let fixture: ComponentFixture<RoutinesComponent>;
  let routineService: jasmine.SpyObj<RoutineService>;
  let router: Router;

  const mockRoutine: Routine = {
    id: '1',
    name: 'Test Routine',
    description: 'Test Description',
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };

  beforeEach(async () => {
    const routineServiceSpy = jasmine.createSpyObj('RoutineService', ['getRoutines', 'deleteRoutine']);
    routineServiceSpy.getRoutines.and.returnValue(of([mockRoutine]));

    await TestBed.configureTestingModule({
      imports: [
        RoutinesComponent,
        CreateRoutineComponent,
        RouterTestingModule
      ],
      providers: [
        { provide: RoutineService, useValue: routineServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => null
              }
            }
          }
        }
      ]
    }).compileComponents();

    routineService = TestBed.inject(RoutineService) as jasmine.SpyObj<RoutineService>;
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(RoutinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
    it('should load routines on init', () => {
      expect(routineService.getRoutines).toHaveBeenCalled();
      expect(component.routines).toEqual([mockRoutine]);
    });

    it('should handle error when loading routines fails', () => {
      const error = new Error('Failed to load routines');
      routineService.getRoutines.and.returnValue(throwError(() => error));
      const consoleSpy = spyOn(console, 'error');

      component.ngOnInit();

      expect(routineService.getRoutines).toHaveBeenCalled();
      expect(consoleSpy).toHaveBeenCalledWith('Error loading routines:', error);
    });
  });

  describe('Routine Management', () => {
    beforeEach(() => {
      component.routines = [mockRoutine];
    });

    it('should open create modal', () => {
      component.openCreateModal();

      expect(component.showCreateModal).toBeTrue();
      expect(component.routineToEdit).toBeUndefined();
    });

    it('should open edit modal', () => {
      component.openEditModal(mockRoutine);

      expect(component.showCreateModal).toBeTrue();
      expect(component.routineToEdit).toEqual(mockRoutine);
    });

    it('should delete routine', () => {
      spyOn(window, 'confirm').and.returnValue(true);
      routineService.deleteRoutine.and.returnValue(of(undefined));

      component.deleteRoutine(mockRoutine.id);

      expect(routineService.deleteRoutine).toHaveBeenCalledWith(mockRoutine.id);
      expect(component.routines).toEqual([]);
    });

    it('should not delete routine if user cancels', () => {
      spyOn(window, 'confirm').and.returnValue(false);

      component.deleteRoutine(mockRoutine.id);

      expect(routineService.deleteRoutine).not.toHaveBeenCalled();
      expect(component.routines).toEqual([mockRoutine]);
    });

    it('should handle error when deleting routine fails', () => {
      spyOn(window, 'confirm').and.returnValue(true);
      const error = new Error('Failed to delete routine');
      routineService.deleteRoutine.and.returnValue(throwError(() => error));
      const consoleSpy = spyOn(console, 'error');

      component.deleteRoutine(mockRoutine.id);

      expect(routineService.deleteRoutine).toHaveBeenCalledWith(mockRoutine.id);
      expect(consoleSpy).toHaveBeenCalledWith('Error deleting routine:', error);
      expect(component.routines).toEqual([mockRoutine]);
    });
  });

  describe('Modal Events', () => {
    beforeEach(() => {
      component.routines = [];
    });

    it('should handle routine created event', () => {
      const newRoutine = { ...mockRoutine, id: '2' };

      component.onRoutineCreated(newRoutine);

      expect(component.routines).toEqual([newRoutine]);
      expect(component.showCreateModal).toBeFalse();
    });

    it('should handle routine updated event', () => {
      const existingRoutine = { ...mockRoutine, name: 'Old Name' };
      const updatedRoutine = { ...mockRoutine, name: 'New Name' };
      component.routines = [existingRoutine];

      component.onRoutineUpdated(updatedRoutine);

      expect(component.routines).toEqual([updatedRoutine]);
      expect(component.showCreateModal).toBeFalse();
    });

    it('should handle modal closed event', () => {
      component.showCreateModal = true;
      component.routineToEdit = mockRoutine;

      component.onModalClosed();

      expect(component.showCreateModal).toBeFalse();
      expect(component.routineToEdit).toBeUndefined();
    });
  });
}); 
