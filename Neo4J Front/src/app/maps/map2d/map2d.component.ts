import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Connection } from 'src/app/maps/models/connection.model';
import { CenterType } from '../enums/center.type.enum';
import { Center } from '../models/center.base.model';
import { DistributionCenter } from '../models/distribution-center.model';
import { Incident } from '../models/incident.model';
import { LocationMap } from '../models/location.model';
import { ResponseDetailTruck } from '../models/response.details.truck.model';
import { Truck } from '../models/truck.model';
import { DistributionCenterService } from '../services/map/distribution-center.service';
import { ShipsService } from '../services/map/ship.service';
import { TrucksService } from '../services/map/trucks.service';
import { MapComponent } from './map/map.component';
import { UtilMapService } from './map/util-map.service';

@Component({
  selector: 'neo4j-map2d',
  templateUrl: './map2d.component.html',
  styleUrls: ['./map2d.component.scss'],
})
export class Map2dComponent implements OnInit {
  @ViewChild('map') map: MapComponent;
  connections: Connection[];
  locations: LocationMap[] = [];
  showMap: boolean;
  showRoute: boolean;
  showTruckProblem: boolean;
  showGenericProblem: boolean;
  showDetails: boolean = false;
  details: DistributionCenter;
  truckDetailShowed: boolean;
  route: any;
  truckId: any;
  incident: Incident;
  truckSelected: Truck;
  destination: Center;
  alreadyMagicPlacesPainted: any;
  idMarkerFrom3D: string = '22';
  centerDistributionDetailsOld: {
    id: string;
    description: string;
    city: string;
    address: string;
    country: string;
    phoneNumber: string;
    planning: number;
    sales: number;
    sourcing: number;
    warehouseAndDistribution: number;
    warehouseAndDistributionTotal: number;
    timeStart: string;
    timeEnd: string;
    reference: string;
    production: any[];
  };
  zoomLevelChangedDetect: any;
  zoomLevel: number;
  markerClickedId: any;
  showCall = false;
  showTrucksCards: boolean = false;
  constructor(
    private activateRoute: ActivatedRoute,
    private router: Router,
    private readonly distributionCenterService: DistributionCenterService,
    private readonly trucksService: TrucksService,
    private readonly shipsService: ShipsService,
    private utilMapCommuService: UtilMapService
  ) {}

  ngOnInit(): void {
    this.idMarkerFrom3D = this.activateRoute.snapshot.paramMap.get('id') || '';
  }

  ngAfterViewInit(): void {
    this.zoomLevelChangedDetect = this.utilMapCommuService.zoomLevel.subscribe(
      (zoomLevel: number) => {
        this.zoomLevel = zoomLevel;
        this.map?.removeAllMarkers();
        this.initViewAllMarkers();
        
      }
    );
    this.initViewAllMarkers();
  }

  onCloseDetails(): void {
    this.showDetails = false;
    this.map.restoreMap(true);
    this.map.removeAllMarkers();
    this.map.removeTruckView();
    this.initViewAllMarkers();
    this.map.removeShips();
  }

  onHiddenDetailsTrucks(): void {
    this.truckDetailShowed = false;
    this.map.isNecesaryAddOpacity = true;
    this.details = this.centerDistributionDetailsOld;
    this.map.restoreMap();
    this.map.removeAllMarkers();
    this.map.removeOpacity();
    this.initViewAllMarkers(true);
  }

  onCloseTruckIncidentModal(): void {
    this.showTruckProblem = false;
  }

  onCloseGenericIncidentModal(): void {
    this.showGenericProblem = false;
  }

  onShowIncident(): void {
    this.trucksService
      .getIncidentByTruckId(this.truckId)
      .subscribe((incident) => {
        this.incident = incident;
        if (incident.id.toString() === '3') {
          this.showTruckProblem = true;
        } else {
          this.showGenericProblem = true;
        }
      });
  }

  onGetDetailsTrucks(idDistributionCenter: any): void {
    this.distributionCenterService
      .getDetailsWithTrucks(idDistributionCenter)
      .subscribe(
        ({
          locations,
          connections,
        }: {
          locations: LocationMap[];
          connections: Connection[];
        }) => {
          this.showTrucksCards = true;
          this.map.saveDetailsMarker();
          this.map.restoreMap();
          this.map.removeAllMarkers();
          this.map.setTruckView();
          this.map.isNecesaryCenterBounds = true;
          this.connections = connections;
          this.locations = locations;
        }
      );
  }

  backToDetails(): void {
    this.showTrucksCards = false;
  }

  onPaintMagicPlaces(magicPlaceResponse: any): void {
    if (!this.alreadyMagicPlacesPainted) {
      this.map.saveDetailsMarker();
      this.map.restoreMap();
    }
    this.map.isNecesaryCenterBounds = true;
    this.map.removeAllMarkers();
    this.alreadyMagicPlacesPainted = true;
    this.connections = magicPlaceResponse.connections;
    this.locations = magicPlaceResponse.locations;
  }

  onTruckClicked(e: any): void {
    this.trucksService
      .getDetailsTruck(e.target.idDistributionCenter)
      .subscribe((res: ResponseDetailTruck) => {
        this.destination = res.centerDestination;
        this.truckSelected = res.truck;
        this.truckId = e.target.idDistributionCenter;
        this.truckDetailShowed = true;
        this.showDetails = false;
        this.showTrucksCards = false;
        this.map.removeTruckView();
        this.route = res.route;
        this.map.addDestination(
          res.centerDestination.position,
          res.truck.position,
          res.route.incidentRoute?.position
        );
      });
  }

  onCloseModalTruckDetail(): void {
    this.truckDetailShowed = false;
    this.showDetails = true;
    this.showTrucksCards = true;
    this.map.removeOpacity();
    this.map.setTruckView();
    this.map.removeAllBorderSelected();
    this.map.removeAdditionalMarker();
  }

  onMarkerClicked(e: any): void {
    this.markerClickedId = e.target.idDistributionCenter;
    if (e.target.type === CenterType.DESTINATION) {
      return;
    }
    if (e.target.type === CenterType.MAGIC_PLACE) {
      this.centerDistributionDetailsOld = { ...this.details };
      this.distributionCenterService
        .getDetailMagicPlace(e.target.customId)
        .subscribe((res: DistributionCenter) => {
          this.details = res;
        });

      return;
    }
    this.distributionCenterService
      .getDetails(e.target.idDistributionCenter)
      .subscribe(
        (res: DistributionCenter) => {
          if (!res) {
            this.map.restoreMap(true);
          }
          localStorage.removeItem('detailsShowed');
          this.details = res;
          this.showDetails = true;

          this.shipsService.getShips().subscribe((res) => {
            this.map.addShips(res);
          });
        },
        (err) => {
          this.map.restoreMap();
        }
      );
  }

  onShowRouteDetailed(): void {
    this.showRoute = true;
  }

  onHiddenRouteDetailed(): void {
    this.showRoute = false;
  }

  private initViewAllMarkers(restore = false): void {
    this.distributionCenterService
      .getLocations(this.zoomLevel)
      .subscribe(({ locations, connections }) => {

        this.locations = locations;
        this.connections = connections;
        this.showMap = true;

        if (restore) {
          this.map.removeTruckView();
          this.map.restoreDetailsMarker();
          this.map.removeShips();
        }
      });
  }
}
