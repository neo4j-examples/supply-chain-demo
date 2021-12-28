import { Component, Input, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { icon, latLng, Map, tileLayer } from 'leaflet';

import { forkJoin } from 'rxjs';
import { MarkerColor } from '../../enums';
import { Center } from '../../models/center.base.model';
import { Truck } from '../../models/truck.model';
import { TrucksService } from '../../services/map/trucks.service';

@Component({
  selector: 'neo4j-route-detailed',
  templateUrl: './route-detailed.component.html',
  styleUrls: ['./route-detailed.component.scss'],
})
export class RouteDetailedComponent implements OnInit {
  mapOptions: any;
  map: Map;
  @Input() truckId: string;
  @Input() truck: Truck;
  @Input() destination: Center;
  hiddenMap = true;
  center: L.LatLng;
  stepByStep: any;

  constructor(private readonly trucksService: TrucksService) {
    this.mapOptions = {
      maxBounds: [
        [90, 180],
        [-90, -180],
      ],
      layers: [
        tileLayer(
          'https://api.mapbox.com/styles/v1/{id}/tiles/256/{z}/{x}/{y}?access_token={accessToken}',
          {
            maxZoom: 18,
            minZoom: 4,
            noWrap: true,
            bounds: [
              [-90, -180],
              [90, 180],
            ],
            id: 'spike-maps/ckv9b2oad610x15nzukiks6ln',
            accessToken:
              'pk.eyJ1Ijoic3Bpa2UtbWFwcyIsImEiOiJja3Y5YWVzcWYxbmZqMzNvazIxMTVtZGNmIn0.ErqbTIynTwkIdrZFee14Tw',
          } as any
        ),
      ],
      zoom: 2,
      center: latLng(33.364929676255805, -117.01039956044674),
    };
  }

  ngOnInit(): void {}

  ngOnDestroy(): void {
    clearInterval(this.stepByStep);
  }

  onMapReady(map: any): void {
    this.map = map;
    this.center = latLng([this.truck.position.lat, this.truck.position.lng]);
    this.map.setZoom(13);
    //this.map.setView();
    forkJoin([
      this.trucksService.getRouteFromOrigin(this.truckId),
      this.trucksService.getRouteToDestination(this.truckId),
    ]).subscribe((res) => {
      const routeOriginTotruck = new L.Polyline(res[0].reverse(), {
        color: '#00BDBD',
        weight: 4,
        opacity: 0.9,
        smoothFactor: 0.2,
      });

      this.map.addLayer(routeOriginTotruck);
      const marker = L.marker(res[0].splice(-1)[0]).setIcon(
        icon({
          iconSize: [50, 50],
          iconAnchor: [50, 50],
          iconUrl: MarkerColor.TRUCK,
        })
      );
      this.map.flyTo([this.truck.position.lat, this.truck.position.lng], 15);
      this.map.addLayer(marker);
      let step = 0;

      this.stepByStep = setInterval(() => {
        if (step < res[1].length) {
          marker.setLatLng(res[1][step]);
          this.map.flyTo(res[1][step]);
          routeOriginTotruck.addLatLng(res[1][step]);
          step++;
        } else {
          clearInterval(this.stepByStep);
        }
      }, 500);
      setTimeout(() => {
        this.hiddenMap = false;
      }, 1000);
    });
  }
}
