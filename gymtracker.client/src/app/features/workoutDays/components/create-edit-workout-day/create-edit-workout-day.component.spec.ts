import { ComponentFixture, TestBed } from '@angular/core/testing';
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
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('when creating a new workout day', () => {
    beforeEach(() => {
      component.workoutDay = mockCreateWorkoutDayDto;
      workoutDayService.createWorkoutDay.and.returnValue(of(mockWorkoutDay));
    });

    it('should emit workoutDayCreated when form is submitted', () => {
      const workoutDayCreatedSpy = spyOn(component.workoutDayCreated, 'emit');
      component.onSubmit();
      expect(workoutDayCreatedSpy).toHaveBeenCalledWith(mockWorkoutDay);
    });

    it('should call createWorkoutDay with correct data', () => {
      component.onSubmit();
      expect(workoutDayService.createWorkoutDay).toHaveBeenCalledWith(mockCreateWorkoutDayDto);
    });

    it('should handle error when creating workout day fails', () => {
      workoutDayService.createWorkoutDay.and.returnValue(throwError(() => new Error('Test Error')));
      component.onSubmit();
      expect(component.error).toBe('Failed to create workout day');
    });
  });

  describe('when editing an existing workout day', () => {
    beforeEach(() => {
      component.workoutDayToEdit = mockWorkoutDay;
      component.ngOnInit();
      component.workoutDay = mockCreateWorkoutDayDto;
      workoutDayService.updateWorkoutDay.and.returnValue(of(mockWorkoutDay));
    });

    it('should initialize form with workout day data', () => {
      expect(component.isEditing).toBeTrue();
      expect(component.workoutDay.name).toBe(mockWorkoutDay.name);
      expect(component.workoutDay.description).toBe(mockWorkoutDay.description);
    });

    it('should emit workoutDayUpdated when form is submitted', () => {
      const workoutDayUpdatedSpy = spyOn(component.workoutDayUpdated, 'emit');
      component.onSubmit();
      expect(workoutDayUpdatedSpy).toHaveBeenCalledWith(mockWorkoutDay);
    });

    it('should call updateWorkoutDay with correct data', () => {
      component.onSubmit();
      expect(workoutDayService.updateWorkoutDay).toHaveBeenCalledWith(
        mockWorkoutDay.id,
        mockCreateWorkoutDayDto
      );
    });

    it('should handle error when updating workout day fails', () => {
      workoutDayService.updateWorkoutDay.and.returnValue(throwError(() => new Error('Test Error')));
      component.onSubmit();
      expect(component.error).toBe('Failed to update workout day');
    });
  });

  describe('form validation', () => {
    it('should not submit if name is empty', () => {
      component.workoutDay = { ...mockCreateWorkoutDayDto, name: '' };
      const workoutDayCreatedSpy = spyOn(component.workoutDayCreated, 'emit');
      component.onSubmit();
      expect(workoutDayCreatedSpy).not.toHaveBeenCalled();
    });

    it('should not submit if routineId is empty', () => {
      component.workoutDay = { ...mockCreateWorkoutDayDto, routineId: '' };
      const workoutDayCreatedSpy = spyOn(component.workoutDayCreated, 'emit');
      component.onSubmit();
      expect(workoutDayCreatedSpy).not.toHaveBeenCalled();
    });
  });

  describe('cancel', () => {
    it('should emit cancel event', () => {
      const cancelSpy = spyOn(component.cancel, 'emit');
      component.onCancel();
      expect(cancelSpy).toHaveBeenCalled();
    });
  });
}); 