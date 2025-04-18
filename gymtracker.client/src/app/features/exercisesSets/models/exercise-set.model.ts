import { Exercise } from '../../exercises/models/exercise.model';

export interface ExerciseSet {
    id: string;
    exercise: Exercise;
    sets: number;
    reps: number;
    weight: number;
    restTime: number;
    createdAt: Date;
    updatedAt: Date;
    isDeleted: boolean;
} 