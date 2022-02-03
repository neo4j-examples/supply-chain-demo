import { Marker } from 'leaflet';
import { CenterType } from '../enums/center.type.enum';

export class CustomMarker extends Marker {
  customId: string;
  idDistributionCenter: number;
  city: string;
  type: CenterType;
}
