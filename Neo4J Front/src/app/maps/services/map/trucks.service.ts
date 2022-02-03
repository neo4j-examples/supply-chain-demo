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
export class TrucksService {
  icon: any = {
    DistributionCenter: MarkerColor.TURQUOISE,
    DestinationCenter: MarkerColor.YELLOW,
    trucks: MarkerColor.TRUCK,
  };

  constructor(private readonly http: HttpClient) {}

  getIncidentByTruckId(idTruck: string): Observable<Incident> {
    return this.http.get<Incident>(
      `${environment.urlApi}/neo4j/incidentdetaisandsolutionfortruckonroute`,
      { params: { idTruck } }
    );
  }

  getDetailsTruck(idTruck: string): Observable<ResponseDetailTruck> {
    return this.http
      .get<ResponseDetailTruck>(
        `${environment.urlApi}/neo4j/distributioncentertruckonroute`,
        {
          params: { idTruck },
        }
      )
      .pipe(
        map(
          ({
            distributionCenterOrigin,
            destination,
            relationShips,
            truck,
            route,
          }: any) => {
            const truckMapped: Truck = truck;
            truckMapped.position = {
              lat: truck.position[0],
              lng: truck.position[1],
            } as Coordinates;
            const responseDetailTruck: ResponseDetailTruck = {
              centerOrigin: distributionCenterOrigin,
              centerDestination: destination,
              relationShips: this.mapToConnection(relationShips),
              truck,
              route,
            };
            return responseDetailTruck;
          }
        )
      );
  }

  getConnections(): Observable<Connection[]> {
    return this.http
      .get<Connection[]>(
        `${environment.urlApi}/neo4j/listcentersandrelations?zoomLevel=1`
      )
      .pipe(
        map((e: any) => {
          const connections: Connection[] = e.relationShips.map(
            (connection: any) => {
              const newConnection: Connection = {
                startPoint: {
                  lat: connection.positionFrom[0],
                  lng: connection.positionFrom[1],
                },
                endPoint: {
                  lat: connection.positionTo[0],
                  lng: connection.positionTo[1],
                },
              };

              return newConnection;
            }
          );

          return connections;
        })
      );
  }

  getDetails(idDistributionCenter: string): Observable<DistributionCenter> {
    return this.http.get<DistributionCenter>(
      `${environment.urlApi}/neo4j/distributioncenterdetails`,
      { params: { idDistributionCenter } }
    );
  }

  getDetailsWithTrucks(
    idDistributionCenter: string
  ): Observable<{ locations: LocationMap[]; connections: Connection[] }> {
    return this.http
      .get<{ locations: LocationMap[]; connections: Connection[] }>(
        `${environment.urlApi}/neo4j/distributioncentertrucks`,
        {
          params: { idDistributionCenter },
        }
      )
      .pipe(
        map((e: any) => {
          const connections: Connection[] = e.relationShips.map(
            (connection: any) => {
              const newConnection: Connection = {
                startPoint: {
                  lat: connection.positionFrom[0],
                  lng: connection.positionFrom[1],
                },
                endPoint: {
                  lat: connection.positionTo[0],
                  lng: connection.positionTo[1],
                },
              };

              return newConnection;
            }
          );

          let locations: LocationMap[] = this.mapResponseToLocation(
            e,
            'trucks',
            'trucks'
          );

          locations.push(this.mapToLocation(e.distributionCenter));

          return { locations, connections };
        })
      );
  }

  getAlternativeRoute(idTruck: string): Observable<Coordinates[]> {
    return this.http
      .get<Coordinates[]>(
        `${environment.urlApi}/neo4j/getpointsforalternativeroutetrucktodestination`,
        {
          params: { idTruck },
        }
      )
      .pipe(
        map((res) => {
          const listPoint = res.map((item: any) => {
            return {
              lat: item[0],
              lng: item[1],
            } as Coordinates;
          });

          return listPoint;
        })
      );
  }
  
  getRouteFromOrigin(idTruck: string): Observable<Coordinates[]> {
    return this.http
      .get<Coordinates[]>(
        `${environment.urlApi}/neo4j/getpointsonroutetruckfromorigin`,
        {
          params: { idTruck },
        }
      )
      .pipe(
        map((res) => {
          const listPoint = res.map((item: any) => {
            return {
              lat: item[0],
              lng: item[1],
            } as Coordinates;
          });

          return listPoint;
        })
      );
  }

  getRouteToDestination(idTruck: string): Observable<Coordinates[]> {
    return this.http
      .get<Coordinates[]>(
        `${environment.urlApi}/neo4j/getpointsonroutetrucktodestination`,
        {
          params: { idTruck },
        }
      )
      .pipe(
        map((res) => {
          const listPoint = res.map((item: any) => {
            return {
              lat: item[0],
              lng: item[1],
            } as Coordinates;
          });

          return listPoint;
        })
      );
  }

  private mapResponseToLocation(
    e: any,
    nodoToExtract: string,
    forceType?: string
  ): LocationMap[] {
    const locations: LocationMap[] = e[nodoToExtract].map((item: any) => {
      return this.mapToLocation(item, forceType);
    });

    return locations;
  }

  private mapToLocation(item: any, forceType?: string): LocationMap {
    const type: string = forceType || item.type;
    const itemMapped: LocationMap = {
      id: item.id,
      name: forceType === 'trucks' ? item.serialNumber : item.city,
      type: forceType || item.type,
      color: this.getColor(item, type),
      position: {
        lat: item.position[0],
        lng: item.position[1],
      } as Coordinates,
    };
    return itemMapped;
  }

  private getColor(item: any, type: string): MarkerColor {
    if (item.hasProblem && type === 'trucks') {
      return MarkerColor.TRUCK_WITH_PROBLEM;
    }

    if (item.hasProblem) {
      return MarkerColor.RED;
    }
    return this.icon[type];
  }

  private mapToConnection(data: any[]): Connection[] {
    return data.map((connection: any) => {
      const newConnection: Connection = {
        startPoint: {
          lat: connection.positionFrom[0],
          lng: connection.positionFrom[1],
        },
        endPoint: {
          lat: connection.positionTo[0],
          lng: connection.positionTo[1],
        },
      };

      return newConnection;
    });
  }
}
