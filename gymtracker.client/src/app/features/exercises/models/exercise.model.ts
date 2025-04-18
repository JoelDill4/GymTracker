import { BodyPart } from "../../bodyParts/models/body-part.model";

export interface Exercise {
  id: string;
  name: string;
  description: string;
  bodyPart: BodyPart;
  createdAt: Date;
  updatedAt: Date;
  isDeleted: boolean;
} 
