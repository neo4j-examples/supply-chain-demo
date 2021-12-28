import { CenterType } from '../enums/center.type.enum';
import { Coordinates } from './coordinates.model';

export interface LocationMap {
  id: number;
  name: string;
  hasProblem?: boolean;
  type: CenterType;
  position: Coordinates;
  color: string;
}
