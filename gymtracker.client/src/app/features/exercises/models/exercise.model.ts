export enum BodyPart {
  Chest = 0,
  Back = 1,
  Shoulders = 2,
  Biceps = 3,
  Triceps = 4,
  Legs = 5,
  Core = 6
}

export interface Exercise {
  id?: number;
  name: string;
  description?: string;
  bodyPart: {
    id: string;
    name: string;
  };
}

export interface CreateExerciseDto {
  name: string;
  description?: string;
  fk_bodypart: string;
} 
