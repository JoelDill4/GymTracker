import { Exercise } from '../../exercises/models/exercise.model';

export interface ExerciseSet {
    id: string;
    exerciseId: string;
    exercise: Exercise;
    order: number;
    reps: number;
    weight: number;
    createdAt: Date;
    updatedAt: Date;
    isDeleted: boolean;
} 
