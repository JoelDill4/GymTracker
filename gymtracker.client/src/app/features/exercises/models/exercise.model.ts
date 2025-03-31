export interface BodyPart {
  id: string;
  name: string;
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
