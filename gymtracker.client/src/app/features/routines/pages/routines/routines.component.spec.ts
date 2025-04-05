import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RoutinesComponent } from './routines.component';
import { RoutineService } from '../../services/routine.service';
import { Routine } from '../../models/routine.model';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CreateRoutineComponent } from '../../components/create-routine/create-routine.component';
import { EditRoutineComponent } from '../../components/edit-routine/edit-routine.component';
import { of, throwError } from 'rxjs';

describe('RoutinesComponent', () => {
  let component: RoutinesComponent;
  let fixture: ComponentFixture<RoutinesComponent>;
  let routineService: jasmine.SpyObj<RoutineService>;

  const mockRoutines: Routine[] = [
    {
      id: '1',
      name: 'Push Day',
      description: 'Chest, shoulders, and triceps',
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false
    },
    {
      id: '2',
      name: 'Pull Day',
      description: 'Back and biceps',
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false
    }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('RoutineService', ['getRoutines', 'deleteRoutine']);
    spy.getRoutines.and.returnValue(of(mockRoutines));
    spy.deleteRoutine.and.returnValue(of(void 0));

    await TestBed.configureTestingModule({
      imports: [
        CommonModule,
        RouterModule,
        CreateRoutineComponent,
        EditRoutineComponent
      ],
      providers: [
        { provide: RoutineService, useValue: spy }
      ]
    }).compileComponents();

    routineService = TestBed.inject(RoutineService) as jasmine.SpyObj<RoutineService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoutinesComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty routines array and modals closed', () => {
    expect(component.routines).toEqual([]);
    expect(component.showCreateModal).toBeFalse();
    expect(component.showEditModal).toBeFalse();
  });

  it('should load routines on init', fakeAsync(() => {
    component.ngOnInit();
    tick();

    expect(routineService.getRoutines).toHaveBeenCalled();
    expect(component.routines).toEqual(mockRoutines);
  }));

  it('should handle error when loading routines fails', fakeAsync(() => {
    const consoleErrorSpy = spyOn(console, 'error');
    routineService.getRoutines.and.returnValue(throwError(() => new Error('Failed to load')));
    
    component.ngOnInit();
    tick();

    expect(routineService.getRoutines).toHaveBeenCalled();
    expect(component.routines).toEqual([]);
    expect(consoleErrorSpy).toHaveBeenCalledWith('Error loading routines:', jasmine.any(Error));
  }));

  it('should prepare routine for editing', () => {
    const routineToEdit = mockRoutines[0];
    component.editRoutine(routineToEdit);

    expect(component.selectedRoutine).toEqual(routineToEdit);
    expect(component.showEditModal).toBeTrue();
  });

  it('should delete routine after confirmation', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.routines = [...mockRoutines];
    const routineToDelete = mockRoutines[0];

    component.deleteRoutine(routineToDelete.id);
    tick();

    expect(routineService.deleteRoutine).toHaveBeenCalledWith(routineToDelete.id);
    expect(component.routines).not.toContain(routineToDelete);
  }));

  it('should not delete routine if not confirmed', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(false);
    component.routines = [...mockRoutines];
    const routineToDelete = mockRoutines[0];

    component.deleteRoutine(routineToDelete.id);
    tick();

    expect(routineService.deleteRoutine).not.toHaveBeenCalled();
    expect(component.routines).toContain(routineToDelete);
  }));

  it('should handle error when deleting routine fails', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(true);
    const consoleErrorSpy = spyOn(console, 'error');
    routineService.deleteRoutine.and.returnValue(throwError(() => new Error('Failed to delete')));
    
    component.routines = [...mockRoutines];
    const routineToDelete = mockRoutines[0];
    const originalRoutines = [...component.routines];

    component.deleteRoutine(routineToDelete.id);
    tick();

    expect(routineService.deleteRoutine).toHaveBeenCalledWith(routineToDelete.id);
    expect(consoleErrorSpy).toHaveBeenCalledWith('Error deleting routine:', jasmine.any(Error));
    expect(component.routines).toEqual(originalRoutines);
  }));

  it('should close create modal and reload routines when routine is created', fakeAsync(() => {
    component.showCreateModal = true;
    const loadRoutinesSpy = spyOn(component, 'loadRoutines');

    component.onRoutineCreated();
    tick();

    expect(component.showCreateModal).toBeFalse();
    expect(loadRoutinesSpy).toHaveBeenCalled();
  }));

  it('should close edit modal and reload routines when routine is updated', fakeAsync(() => {
    component.showEditModal = true;
    const loadRoutinesSpy = spyOn(component, 'loadRoutines');

    component.onRoutineUpdated();
    tick();

    expect(component.showEditModal).toBeFalse();
    expect(loadRoutinesSpy).toHaveBeenCalled();
  }));
}); 