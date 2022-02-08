import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as L from 'leaflet';
import { latLng, Map, tileLayer } from 'leaflet';
import { Incident } from '../../models/incident.model';
import { Truck } from '../../models/truck.model';
import { TrucksService } from '../../services/map/trucks.service';

@Component({
  selector: 'neo4j-generic-problem-modal',
  templateUrl: './generic-problem-modal.component.html',
  styleUrls: ['./generic-problem-modal.component.scss'],
})
export class GenericProblemModalComponent implements OnInit {
  @Input() truckInfo: Truck;
  @Input() incident: Incident;
  @Output() closeModal = new EventEmitter<void>();
  @Output() callDriver = new EventEmitter<void>();
  map: Map;
  mapOptions: any;

  constructor(private truckService: TrucksService) {
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
            minZoom: 1,
            noWrap: true,
            bounds: [
              [-90, -180],
              [90, 180],
            ],
            id: 'xxxxx',
            accessToken:
              'xxxxxxx',
          } as any
        ),
      ],
      zoom: 2,
      center: latLng(45, 30),
    };
  }

  ngOnInit(): void {



  }

  closeClicked(): void {
    this.closeModal.emit();
  }

  onMapReady(map: any): void {
    this.map = map;
    this.truckService.getAlternativeRoute(this.truckInfo.id).subscribe(res => {
 
      const routeOriginTotruck = new L.Polyline(res.reverse(), {
        color: '#00BDBD',
        weight: 4,
        opacity: 0.9,
        smoothFactor: 0.2,
      });

      this.map.addLayer(routeOriginTotruck);
      this.map.flyTo([res[0].lat, res[0].lng], 8);
    });
  }
}
