import { WorkoutDay } from '../../workoutDays/models/workoutday.model';
import { ExerciseSet } from '../../exercisesSets/models/exercise-set.model';

export interface Workout {
    id: string;
    workoutDate: Date;
    observations: string;
    workoutDay: WorkoutDay;
    exerciseSets: ExerciseSet[];
    createdAt: Date;
    updatedAt: Date;
    isDeleted: boolean;
} 