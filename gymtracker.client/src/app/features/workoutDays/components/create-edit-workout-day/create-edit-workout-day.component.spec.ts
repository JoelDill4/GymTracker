import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CreateWorkoutDayComponent } from './create-edit-workout-day.component';
import { WorkoutDayService } from '../../services/workoutday.service';
import { of, throwError } from 'rxjs';
import { WorkoutDay, CreateWorkoutDayDto } from '../../models/workoutday.model';

describe('CreateWorkoutDayComponent', () => {
  let component: CreateWorkoutDayComponent;
  let fixture: ComponentFixture<CreateWorkoutDayComponent>;
  let workoutDayService: jasmine.SpyObj<WorkoutDayService>;

  const mockWorkoutDay: WorkoutDay = {
    id: '1',
    name: 'Test Workout Day',
    description: 'Test Description',
    createdAt: new Date(),
    updatedAt: new Date()
  };

  const mockCreateWorkoutDayDto: CreateWorkoutDayDto = {
    name: 'Test Workout Day',
    description: 'Test Description',
    routineId: '1'
  };

  beforeEach(async () => {
    const workoutDayServiceSpy = jasmine.createSpyObj('WorkoutDayService', [
      'createWorkoutDay',
      'updateWorkoutDay'
    ]);

    await TestBed.configureTestingModule({
      imports: [CreateWorkoutDayComponent],
      providers: [
        { provide: WorkoutDayService, useValue: workoutDayServiceSpy }
      ]
    }).compileComponents();

    workoutDayService = TestBed.inject(WorkoutDayService) as jasmine.SpyObj<WorkoutDayService>;
    fixture = TestBed.createComponent(CreateWorkoutDayComponent);
    component = fixture.componentInstance;
    component.routineId = '1';
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
    it('should initialize with empty workout day in create mode', () => {
      expect(component.workoutDay).toEqual({
        name: '',
        description: '',
        routineId: '1'
      });
      expect(component.isEditing).toBeFalse();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });

    it('should initialize with provided workout day in edit mode', () => {
      component.workoutDayToEdit = mockWorkoutDay;
      component.ngOnInit();
      
      expect(component.workoutDay).toEqual({
        name: mockWorkoutDay.name,
        description: mockWorkoutDay.description,
        routineId: '1'
      });
      expect(component.isEditing).toBeTrue();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    });
  });

  describe('Form Validation', () => {
    it('should not submit if name is empty', fakeAsync(() => {
      component.workoutDay.name = '';
      component.workoutDay.description = 'Test Description';
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(workoutDayService.createWorkoutDay).not.toHaveBeenCalled();
      expect(component.error).toBe('Name is required');
    }));

    it('should not submit if routineId is empty', fakeAsync(() => {
      component.workoutDay.name = 'Test Workout Day';
      component.workoutDay.routineId = '';
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');
      
      component.onSubmit();
      tick();
      
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(workoutDayService.createWorkoutDay).not.toHaveBeenCalled();
      expect(component.error).toBe('Routine ID is required');
    }));
  });

  describe('Create Mode', () => {
    it('should create workout day and emit created event on successful submit', fakeAsync(() => {
      workoutDayService.createWorkoutDay.and.returnValue(of(mockWorkoutDay));
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.workoutDay = mockCreateWorkoutDayDto;
      component.onSubmit();
      tick();

      expect(workoutDayService.createWorkoutDay).toHaveBeenCalledWith(mockCreateWorkoutDayDto);
      expect(createdSpy).toHaveBeenCalledWith(mockWorkoutDay);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when creating workout day fails', fakeAsync(() => {
      workoutDayService.createWorkoutDay.and.returnValue(throwError(() => new Error('Creation failed')));
      const createdSpy = spyOn(component.created, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.workoutDay = mockCreateWorkoutDayDto;
      component.onSubmit();
      tick();

      expect(workoutDayService.createWorkoutDay).toHaveBeenCalledWith(mockCreateWorkoutDayDto);
      expect(createdSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to create workout day');
    }));
  });

  describe('Edit Mode', () => {
    beforeEach(() => {
      component.workoutDayToEdit = mockWorkoutDay;
      component.ngOnInit();
    });

    it('should update workout day and emit updated event on successful submit', fakeAsync(() => {
      const updatedWorkoutDay = { ...mockWorkoutDay, name: 'Updated Workout Day' };
      workoutDayService.updateWorkoutDay.and.returnValue(of(updatedWorkoutDay));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.workoutDay.name = 'Updated Workout Day';
      component.onSubmit();
      tick();

      expect(workoutDayService.updateWorkoutDay).toHaveBeenCalledWith(
        mockWorkoutDay.id,
        component.workoutDay
      );
      expect(updatedSpy).toHaveBeenCalledWith(updatedWorkoutDay);
      expect(cancelSpy).toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('');
    }));

    it('should handle error when updating workout day fails', fakeAsync(() => {
      workoutDayService.updateWorkoutDay.and.returnValue(throwError(() => new Error('Update failed')));
      const updatedSpy = spyOn(component.updated, 'emit');
      const cancelSpy = spyOn(component.cancel, 'emit');

      component.workoutDay.name = 'Updated Workout Day';
      component.onSubmit();
      tick();

      expect(workoutDayService.updateWorkoutDay).toHaveBeenCalledWith(
        mockWorkoutDay.id,
        component.workoutDay
      );
      expect(updatedSpy).not.toHaveBeenCalled();
      expect(cancelSpy).not.toHaveBeenCalled();
      expect(component.loading).toBeFalse();
      expect(component.error).toBe('Failed to update workout day');
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
