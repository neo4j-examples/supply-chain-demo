import { Coordinates } from './coordinates.model';

export interface Truck {
  id: string;
  serialNumber: string;
  hasProbleml: boolean;
  position: Coordinates;
}
