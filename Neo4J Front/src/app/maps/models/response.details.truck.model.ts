import { Center } from './center.base.model';
import { Connection } from './connection.model';
import { Route } from './route.model';
import { Truck } from './truck.model';

export interface ResponseDetailTruck {
  centerOrigin: Center;
  centerDestination: Center;
  truck: Truck;
  route: Route;
  relationShips: Connection[];
}
