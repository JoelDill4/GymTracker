import { Routine } from '../../routines/models/routine.model';

export interface WorkoutDay {
    id: string;
    name: string;
    description: string;
    routine: Routine;
    createdAt: Date;
    updatedAt: Date;
    isDeleted: boolean;
} 
