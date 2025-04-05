import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CreateRoutineComponent } from './create-routine.component';
import { RoutineService } from '../../services/routine.service';
import { FormsModule } from '@angular/forms';
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
    const routineSpy = jasmine.createSpyObj('RoutineService', ['createRoutine']);
    routineSpy.createRoutine.and.returnValue(of(mockRoutine));

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      providers: [
        { provide: RoutineService, useValue: routineSpy }
      ]
    }).compileComponents();

    routineService = TestBed.inject(RoutineService) as jasmine.SpyObj<RoutineService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateRoutineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty routine', () => {
    expect(component.routine).toEqual({
      name: '',
      description: ''
    });
  });

  it('should emit close event when close button is clicked', () => {
    const closeSpy = spyOn(component.close, 'emit');
    const closeButton = fixture.nativeElement.querySelector('.btn-close');
    
    closeButton.click();
    
    expect(closeSpy).toHaveBeenCalled();
  });

  it('should emit close event when cancel button is clicked', () => {
    const closeSpy = spyOn(component.close, 'emit');
    const cancelButton = fixture.nativeElement.querySelector('.btn-secondary');
    
    cancelButton.click();
    
    expect(closeSpy).toHaveBeenCalled();
  });

  it('should create routine and emit created event on successful submit', fakeAsync(() => {
    const createdSpy = spyOn(component.created, 'emit');
    component.routine = {
      name: 'New Routine',
      description: 'New Description'
    };

    component.onSubmit();
    tick();

    expect(routineService.createRoutine).toHaveBeenCalledWith({
      name: 'New Routine',
      description: 'New Description'
    });
    expect(createdSpy).toHaveBeenCalled();
  }));

  it('should handle error when creating routine fails', fakeAsync(() => {
    const error = new Error('Failed to create routine');
    routineService.createRoutine.and.returnValue(throwError(() => error));
    const consoleSpy = spyOn(console, 'error');
    const createdSpy = spyOn(component.created, 'emit');
    
    component.routine = {
      name: 'New Routine',
      description: 'New Description'
    };

    component.onSubmit();
    tick();

    expect(consoleSpy).toHaveBeenCalledWith('Error creating routine:', error);
    expect(createdSpy).not.toHaveBeenCalled();
  }));
}); 
