import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { EditRoutineComponent } from './edit-routine.component';
import { RoutineService } from '../../services/routine.service';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { Routine } from '../../models/routine.model';

describe('EditRoutineComponent', () => {
  let component: EditRoutineComponent;
  let fixture: ComponentFixture<EditRoutineComponent>;
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
    const routineSpy = jasmine.createSpyObj('RoutineService', ['updateRoutine']);
    routineSpy.updateRoutine.and.returnValue(of(mockRoutine));

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      providers: [
        { provide: RoutineService, useValue: routineSpy }
      ]
    }).compileComponents();

    routineService = TestBed.inject(RoutineService) as jasmine.SpyObj<RoutineService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditRoutineComponent);
    component = fixture.componentInstance;
    component.routine = { ...mockRoutine };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with provided routine data', () => {
    expect(component.routine).toEqual(mockRoutine);
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

  it('should update routine and emit updated event on successful submit', fakeAsync(() => {
    const updatedSpy = spyOn(component.updated, 'emit');
    component.routine = {
      ...mockRoutine,
      name: 'Updated Routine',
      description: 'Updated Description'
    };

    component.onSubmit();
    tick();

    expect(routineService.updateRoutine).toHaveBeenCalledWith('1', {
      name: 'Updated Routine',
      description: 'Updated Description'
    });
    expect(updatedSpy).toHaveBeenCalled();
  }));

  it('should handle error when updating routine fails', fakeAsync(() => {
    const error = new Error('Failed to update routine');
    routineService.updateRoutine.and.returnValue(throwError(() => error));
    const consoleSpy = spyOn(console, 'error');
    const updatedSpy = spyOn(component.updated, 'emit');
    
    component.routine = {
      ...mockRoutine,
      name: 'Updated Routine',
      description: 'Updated Description'
    };

    component.onSubmit();
    tick();

    expect(consoleSpy).toHaveBeenCalledWith('Error updating routine:', error);
    expect(updatedSpy).not.toHaveBeenCalled();
  }));
  
  it('should display current routine data in form fields', () => {
    component.routine = mockRoutine;
    fixture.detectChanges();
    
    const nameInput = fixture.nativeElement.querySelector('#name') as HTMLInputElement;
    const descriptionTextarea = fixture.nativeElement.querySelector('#description') as HTMLTextAreaElement;
    
    fixture.whenStable().then(() => {
      expect(nameInput.value).toBe(mockRoutine.name);
      expect(descriptionTextarea.value).toBe(mockRoutine.description);
    });
  });
}); 
