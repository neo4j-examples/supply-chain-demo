import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MarkerColor } from '../../enums';
import { Connection } from '../../models/connection.model';
import { Coordinates } from '../../models/coordinates.model';
import { DistributionCenter } from '../../models/distribution-center.model';
import { Incident } from '../../models/incident.model';
import { LocationMap } from '../../models/location.model';
import { ResponseDetailTruck } from '../../models/response.details.truck.model';
import { Truck } from '../../models/truck.model';

@Injectable({
  providedIn: 'root',
})
export class ShipsService {
  icon: any = {
    DistributionCenter: MarkerColor.TURQUOISE,
    DestinationCenter: MarkerColor.YELLOW,
    trucks: MarkerColor.TRUCK,
  };

  constructor(private readonly http: HttpClient) {}

  getShips(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.urlApi}/neo4j/getships`);
  }

}
