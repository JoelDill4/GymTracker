import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CreateExerciseComponent } from './create-exercise.component';
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

  beforeEach(async () => {
    const exerciseSpy = jasmine.createSpyObj('ExerciseService', ['createExercise']);
    const bodyPartSpy = jasmine.createSpyObj('BodyPartService', ['getBodyParts']);
    bodyPartSpy.getBodyParts.and.returnValue(of(mockBodyParts));

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      providers: [
        { provide: ExerciseService, useValue: exerciseSpy },
        { provide: BodyPartService, useValue: bodyPartSpy }
      ]
    }).compileComponents();

    exerciseService = TestBed.inject(ExerciseService) as jasmine.SpyObj<ExerciseService>;
    bodyPartService = TestBed.inject(BodyPartService) as jasmine.SpyObj<BodyPartService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateExerciseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty exercise and load body parts', () => {
    expect(component.exercise).toEqual({
      name: '',
      description: '',
      bodyPart: {
        id: '',
        name: ''
      }
    });
    expect(component.selectedBodyPartId).toBe('');
    expect(bodyPartService.getBodyParts).toHaveBeenCalled();
  });

  it('should load body parts on init', fakeAsync(() => {
    component.ngOnInit();
    tick();

    expect(bodyPartService.getBodyParts).toHaveBeenCalled();
    expect(component.bodyParts).toEqual(mockBodyParts);
  }));

  it('should emit cancel event when onCancel is called', () => {
    spyOn(component.cancel, 'emit');
    component.onCancel();
    expect(component.cancel.emit).toHaveBeenCalled();
  });

  it('should create exercise and emit event on successful submit', fakeAsync(() => {
    const newExercise: Exercise = {
      id: 1,
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
    spyOn(component.exerciseCreated, 'emit');

    component.onSubmit();
    tick();

    expect(exerciseService.createExercise).toHaveBeenCalledWith({
      name: 'Bench Press',
      description: 'Chest exercise',
      fk_bodypart: '1'
    });
    expect(component.exerciseCreated.emit).toHaveBeenCalledWith(newExercise);
  }));

  it('should not submit if name or body part is missing', fakeAsync(() => {
    component.exercise.name = '';
    component.selectedBodyPartId = '';

    component.onSubmit();
    tick();

    expect(exerciseService.createExercise).not.toHaveBeenCalled();
  }));

  it('should handle error when creating exercise fails', fakeAsync(() => {
    component.exercise.name = 'Bench Press';
    component.exercise.description = 'Chest exercise';
    component.selectedBodyPartId = '1';

    exerciseService.createExercise.and.returnValue(throwError(() => new Error('Failed to create')));
    spyOn(console, 'error');
    spyOn(component.exerciseCreated, 'emit');

    component.onSubmit();
    tick();

    expect(exerciseService.createExercise).toHaveBeenCalled();
    expect(component.exerciseCreated.emit).not.toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error creating exercise:', jasmine.any(Error));
  }));
}); 
