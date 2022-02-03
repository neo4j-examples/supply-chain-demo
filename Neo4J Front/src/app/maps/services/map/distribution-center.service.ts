import { CenterType } from './../../enums/center.type.enum';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MarkerColor } from '../../enums';
import { Connection } from '../../models/connection.model';
import { Coordinates } from '../../models/coordinates.model';
import { DistributionCenter } from '../../models/distribution-center.model';
import { LocationMap } from '../../models/location.model';

@Injectable({
  providedIn: 'root',
})
export class DistributionCenterService {

  icon: any = {
    DistributionCenter: MarkerColor.TURQUOISE,
    DestinationCenter: MarkerColor.YELLOW,
    trucks: MarkerColor.TRUCK,
    magicPlaces: MarkerColor.MAGIC_PLACE,
  };

  constructor(private readonly http: HttpClient) {}

  getLocations(zoomLevel = 1): Observable<{
    locations: LocationMap[];
    connections: Connection[];
  }> {
    return this.http
      .get<{ locations: LocationMap[]; connections: Connection[] }>(
        `${environment.urlApi}/neo4j/listcentersandrelations?zoomLevel=${zoomLevel}`
      )
      .pipe(
        map((response: any) => {
          let locations: LocationMap[] = this.mapResponseToLocation(
            response,
            'distributionCenters'
          );

          locations = [
            ...locations,
            ...this.mapResponseToLocation(response, 'destinationCenters'),
          ];

          const connections: Connection[] = response.relationShips.map(
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

          return { locations, connections };
        })
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

  getDetailsWithTrucks(idDistributionCenter: string): Observable<any> {
    return this.http
      .get<any>(`${environment.urlApi}/neo4j/distributioncentertrucks`, {
        params: { idDistributionCenter },
      })
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

  getVariablesForMagicPlaces(): Observable<
    { id: number; description: string }[]
  > {
    return this.http.get<{ id: number; description: string }[]>(
      `${environment.urlApi}/neo4j/getmagicplacefeatureslist`
    );
  }

  getDetailMagicPlace(idMagicPlace: string) {
    return this.http.get<DistributionCenter>(
      `${environment.urlApi}/neo4j/magicplacedetails`,
      { params: { idMagicPlace } }
    );
  }

  getMagicPlaces(feature: any): Observable<any> {
    return this.http
      .post<any>(
        `${environment.urlApi}/neo4j/distributioncentermagicplaces`,
        feature
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
            "magicPlaces",
            CenterType.MAGIC_PLACE
          );

          locations.push(this.mapToLocation(e.distributionCenter));

          return { locations, connections };
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
      name: this.getName(forceType, item),
      type: forceType || item.type,
      hasProblem: item.hasProblem,
      color: this.getColor(item, type),
      position: {
        lat: item.position[0],
        lng: item.position[1],
      } as Coordinates,
    };
    return itemMapped;
  }
  getName(forceType: string | undefined, item: any): string {
    if (forceType === 'trucks') {
      return item.serialNumber;
    }
    if (forceType === CenterType.MAGIC_PLACE) {
      return 'Magic Place';
    }
    return item.city;
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
}
