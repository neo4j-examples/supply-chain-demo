import { CenterType } from '../enums/center.type.enum';

export interface Center {
  id: string;
  description: string;
  city: string;
  position: number[];
  type: CenterType;
  hasProblem: boolean;
}
