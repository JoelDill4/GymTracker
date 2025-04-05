import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RoutineDetailComponent } from './routine-detail.component';
import { RoutineService } from '../../services/routine.service';
import { WorkoutDayService } from '../../../workoutDays/services/workoutday.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateWorkoutDayComponent } from '../../../workoutDays/components/create-workout-day/create-workout-day.component';
import { of, throwError } from 'rxjs';
import { Routine } from '../../models/routine.model';
import { WorkoutDay } from '../../../workoutDays/models/workoutday.model';

describe('RoutineDetailComponent', () => {
  let component: RoutineDetailComponent;
  let fixture: ComponentFixture<RoutineDetailComponent>;
  let routineService: jasmine.SpyObj<RoutineService>;
  let workoutDayService: jasmine.SpyObj<WorkoutDayService>;

  const mockRoutine: Routine = {
    id: '1',
    name: 'Test Routine',
    description: 'Test Description',
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false
  };

  const mockWorkoutDays: WorkoutDay[] = [
    {
      id: '1',
      name: 'Day 1',
      description: 'Push Day',
      routine: mockRoutine,
      createdAt: new Date()
    },
    {
      id: '2',
      name: 'Day 2',
      description: 'Pull Day',
      routine: mockRoutine,
      createdAt: new Date()
    }
  ];

  beforeEach(async () => {
    const routineSpy = jasmine.createSpyObj('RoutineService', ['getRoutine', 'getWorkoutDaysByRoutine']);
    routineSpy.getRoutine.and.returnValue(of(mockRoutine));
    routineSpy.getWorkoutDaysByRoutine.and.returnValue(of(mockWorkoutDays));

    const workoutDaySpy = jasmine.createSpyObj('WorkoutDayService', ['deleteWorkoutDay']);
    workoutDaySpy.deleteWorkoutDay.and.returnValue(of(void 0));

    await TestBed.configureTestingModule({
      imports: [
        CommonModule,
        FormsModule,
        CreateWorkoutDayComponent
      ],
      providers: [
        { provide: RoutineService, useValue: routineSpy },
        { provide: WorkoutDayService, useValue: workoutDaySpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ id: '1' })
          }
        }
      ]
    }).compileComponents();

    routineService = TestBed.inject(RoutineService) as jasmine.SpyObj<RoutineService>;
    workoutDayService = TestBed.inject(WorkoutDayService) as jasmine.SpyObj<WorkoutDayService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoutineDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty routine and workout days', () => {
    const newComponent = new RoutineDetailComponent(
      TestBed.inject(ActivatedRoute),
      routineService,
      workoutDayService
    );
    expect(newComponent.routine).toBeNull();
    expect(newComponent.workoutDays).toEqual([]);
    expect(newComponent.loading).toBeFalse();
    expect(newComponent.error).toBeNull();
    expect(newComponent.showCreateModal).toBeFalse();
  });

  it('should load routine and workout days on init', fakeAsync(() => {
    component.ngOnInit();
    tick();

    expect(routineService.getRoutine).toHaveBeenCalledWith('1');
    expect(routineService.getWorkoutDaysByRoutine).toHaveBeenCalledWith('1');
    expect(component.routine).toEqual(mockRoutine);
    expect(component.workoutDays).toEqual(mockWorkoutDays);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should handle error when loading routine fails', fakeAsync(() => {
    const error = new Error('Failed to load routine');
    routineService.getRoutine.and.returnValue(throwError(() => error));
    const consoleSpy = spyOn(console, 'error');

    component.ngOnInit();
    tick();

    expect(consoleSpy).toHaveBeenCalledWith('Error loading routine:', error);
    expect(component.error).toBe('Failed to load routine');
  }));

  it('should handle error when loading workout days fails', fakeAsync(() => {
    const error = new Error('Failed to load workout days');
    routineService.getWorkoutDaysByRoutine.and.returnValue(throwError(() => error));
    const consoleSpy = spyOn(console, 'error');

    component.ngOnInit();
    tick();

    expect(consoleSpy).toHaveBeenCalledWith('Error loading workout days:', error);
    expect(component.error).toBe('Failed to load workout days');
    expect(component.loading).toBeFalse();
  }));

  it('should show create modal when onCreateWorkoutDay is called', () => {
    component.onCreateWorkoutDay();
    expect(component.showCreateModal).toBeTrue();
  });

  it('should hide create modal when onCancelCreate is called', () => {
    component.showCreateModal = true;
    component.onCancelCreate();
    expect(component.showCreateModal).toBeFalse();
  });

  it('should add new workout day and hide modal when onWorkoutDayCreated is called', () => {
    const newWorkoutDay: WorkoutDay = {
      id: '3',
      name: 'Day 3',
      description: 'Legs Day',
      routine: mockRoutine,
      createdAt: new Date()
    };

    component.showCreateModal = true;
    component.workoutDays = [...mockWorkoutDays];
    component.onWorkoutDayCreated(newWorkoutDay);

    expect(component.workoutDays).toContain(newWorkoutDay);
    expect(component.showCreateModal).toBeFalse();
  });

  it('should delete workout day after confirmation', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.workoutDays = [...mockWorkoutDays];
    const workoutDayToDelete = mockWorkoutDays[0];

    component.deleteWorkoutDay(workoutDayToDelete.id);
    tick();

    expect(workoutDayService.deleteWorkoutDay).toHaveBeenCalledWith(workoutDayToDelete.id);
    expect(component.workoutDays).not.toContain(workoutDayToDelete);
  }));

  it('should not delete workout day if not confirmed', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(false);
    component.workoutDays = [...mockWorkoutDays];
    const workoutDayToDelete = mockWorkoutDays[0];

    component.deleteWorkoutDay(workoutDayToDelete.id);
    tick();

    expect(workoutDayService.deleteWorkoutDay).not.toHaveBeenCalled();
    expect(component.workoutDays).toContain(workoutDayToDelete);
  }));

  it('should handle error when deleting workout day fails', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(true);
    const error = new Error('Failed to delete workout day');
    workoutDayService.deleteWorkoutDay.and.returnValue(throwError(() => error));
    const consoleSpy = spyOn(console, 'error');
    
    component.workoutDays = [...mockWorkoutDays];
    const workoutDayToDelete = mockWorkoutDays[0];
    const originalWorkoutDays = [...component.workoutDays];

    component.deleteWorkoutDay(workoutDayToDelete.id);
    tick();

    expect(workoutDayService.deleteWorkoutDay).toHaveBeenCalledWith(workoutDayToDelete.id);
    expect(consoleSpy).toHaveBeenCalledWith('Error deleting workout day:', error);
    expect(component.error).toBe('Failed to delete workout day');
    expect(component.workoutDays).toEqual(originalWorkoutDays);
  }));
}); 