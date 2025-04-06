export interface BodyPart {
  id: string;
  name: string;
}

export interface Exercise {
  id?: string;
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
  fk_bodyPart: string;
} 
