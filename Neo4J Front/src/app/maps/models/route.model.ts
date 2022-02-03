import { ItemRoute } from './item-route.model';

export interface Route {
  idTruck: string;
  serialNumberTruck: string;
  distributionCenterAddress: string;
  distributionCenterCity: string;
  distributionCenterCountry: string;
  distributionCenterPhoneNumber: string;
  distributionCenterTimeSchedule: string;
  originRoute: ItemRoute;
  destinationRoute: ItemRoute;
  incidentRoute: ItemRoute;
}
