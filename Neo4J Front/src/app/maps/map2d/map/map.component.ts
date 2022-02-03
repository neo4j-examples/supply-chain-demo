import { SearcherService } from './../../../core/services/searcher.service';
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  NgZone,
  OnInit,
  Output,
  Renderer2,
} from '@angular/core';
import * as L from 'leaflet';
import {
  icon,
  LatLng,
  latLng,
  LatLngExpression,
  Map,
  tileLayer,
} from 'leaflet';
import { MarkerColor } from '../../enums';
import { CenterType } from '../../enums/center.type.enum';
import { Connection } from '../../models/connection.model';
import { Coordinates } from '../../models/coordinates.model';
import { CustomMarker } from '../../models/customMarker';
import { LocationMap } from '../../models/location.model';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';
import { UtilMapService } from './util-map.service';

@Component({
  selector: 'neo4j-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
})
export class MapComponent implements OnInit {
  connectionsLayers: L.Polyline[] = [];
  markerSelected: any;
  detailsTrucksShowed: boolean;

  _connections: Connection[];
  markerSelectedOld: any;
  iconOld: any;
  destinationLayers: any;
  tooltipLine: any;
  tooltip: any;
  tooltipName: any;
  clickedOnMarker: boolean;
  borderSelected: any;
  containerParent: any;
  idMarker: string;
  isNecesaryAddOpacity: boolean;
  idMarkerOld: string;
  isNecesaryCenterBounds: boolean;
  shipsLayers: CustomMarker[] = [];
  @Input() idMarkerFrom3D: string;
  disabledInteractions: boolean;
  get connections(): Connection[] {
    return this._connections;
  }
  @Input() set connections(value: Connection[]) {
    this._connections = value;
    if (value && this.map) {
      this.paintConnection(value);
    }
  }

  _places: LocationMap[];
  get places(): LocationMap[] {
    return this._places;
  }

  @Input() set places(value: LocationMap[]) {
    this._places = value;
    if (value && this.map) {
      this.paintMarkers(value);
    }
  }

  @Output() markerClick = new EventEmitter<any>();
  @Output() truckClicked = new EventEmitter<any>();
  map: Map;
  mapOptions: any;
  isEndFlyTo: boolean;
  initialMap: {
    markers: CustomMarker[];
    connections: L.Polyline[];
    idMarkerSelected: string;
    layers: any[];
  } = { markers: [], idMarkerSelected: '', connections: [], layers: [] };



  constructor(
    private searcherService: SearcherService,
    private zone: NgZone,
    private renderer: Renderer2,
    private elem: ElementRef,
    private utilMapCommuService: UtilMapService
  ) {
    this.initializeMap();
  }

  createTooltipElement(): void {
    this.tooltip = this.renderer.createElement('div');
    this.tooltipLine = this.renderer.createElement('img');
    this.tooltipName = this.renderer.createElement('span');
    this.renderer.setAttribute(this.tooltip, 'id', 'tooltip-id');
    this.renderer.setAttribute(
      this.tooltipLine,
      'src',
      'assets/images/map/border_city.svg'
    );
    this.renderer.addClass(this.tooltip, 'custom-tooltip-map-name-border-dos');
    this.renderer.addClass(this.tooltipName, 'custom-tooltip-map-name-city');
    this.renderer.appendChild(this.tooltip, this.tooltipLine);
    this.renderer.appendChild(this.tooltip, this.tooltipName);
  }

  ngOnInit(): void {
    this.createTooltipElement();
    this.borderSelected = this.renderer.createElement('img');

    this.renderer.setAttribute(
      this.borderSelected,
      'src',
      'assets/images/map/border_problem.svg'
    );

    this.renderer.addClass(
      this.borderSelected,
      'custom-detail-modal-marker-border'
    );
  }

  onMapReady(map: any): void {
    this.map = map;
    this.paintMarkers(this.places);
    this.paintConnection(this.connections);
    this.map.on('zoomend', (e) => {

      if (this.clickedOnMarker) {
        this.addBorderSelected();
      }else{
        this.removeTooltip();
        const zoomLevel = this.map.getZoom();
        if(zoomLevel < 5) {
          this.utilMapCommuService.zoomLevelSubject.next(1);
        }else {
          this.utilMapCommuService.zoomLevelSubject.next(2);
        }
        
      }
      this.clickedOnMarker = false;
    });

    this.renderer.selectRootElement('.leaflet-control-attribution');
    this.renderer.selectRootElement('.leaflet-control-zoom');
    if (this.idMarkerFrom3D) {
      this.fireClick(this.idMarkerFrom3D);
    }
    this.searcherService.searched.subscribe((location) => {
      if (this.disabledInteractions) {
        return;
      }
      this.map.flyTo(latLng(location[0], location[1]), 8);
    });
  }
  paintMarkers(places: LocationMap[]) {
    for (const item of places) {
      this.addMarker(
        item.name,
        item.id,
        item.type,
        new LatLng(item.position.lat, item.position.lng),
        item.color
      );
    }
  }
  setTruckView() {
    this.detailsTrucksShowed = true;
    this.removeTooltip();
    this.markerSelected = null as any;
    this.idMarker = '';
  }

  removeTruckView() {
    this.detailsTrucksShowed = false;
  }

  removeAllBorderSelected(): void {
    try {
      this.renderer.removeChild(this.containerParent, this.borderSelected);
      this.markerSelected = null;
      this.idMarker = '';
    } catch {
      return;
    }
  }

  removeAllMarkers(): void {
    for (let layer of this.initialMap.connections) {
      this.map.removeLayer(layer);
    }

    for (let marker of this.initialMap.markers) {
      this.map.removeLayer(marker);
    }
    this.initialMap.connections = [];
    this.initialMap.markers = [];
    this.markerSelected = {} as any;
    this.removeAllBorderSelected();
  }

  centerMapBounds(removeSelected: boolean = true): void {
    if(removeSelected) this.removeAllBorderSelected();
    const group = L.featureGroup(this.initialMap.markers);
    this.clickedOnMarker = true;
    this.map.flyToBounds(group.getBounds(), { maxZoom: 8 });
  }

  saveDetailsMarker(): void {
    this.markerSelectedOld = this.markerSelected;
    this.iconOld = this.markerSelected.target._icon;
    this.idMarkerOld = this.idMarker;
    this.markerSelected = null as any;
  }

  restoreDetailsMarker(): void {
    this.markerSelectedOld.target._icon = this.iconOld;
    this.clickedOnMarker = true;
    this.idMarker = this.idMarkerOld;
    this.clickMarker(this.markerSelectedOld);
  }

  removeAdditionalMarker(): void {
    this.map.removeLayer(this.destinationLayers.destinationMarker);
    this.map.removeLayer(this.destinationLayers.connection);
    this.map.removeLayer(this.destinationLayers.incident);
    this.destinationLayers = {};
  }

  addShips(ships: any[]): void {
    if(this.shipsLayers.length > 0) {
      return;
    }
    for (const ship of ships) {
      const shipMarker = L.marker(
        new LatLng(ship.positions[0][0], ship.positions[0][1])
      ).setIcon(
        icon({
          iconSize: [25, 10],
          iconAnchor: [25, 10],
          iconUrl: MarkerColor.SHIP,
        })
      ) as CustomMarker;
      shipMarker.customId = ship.id;

      this.shipsLayers.push(shipMarker);
      this.map.addLayer(shipMarker);
    }

    this.shipsLayers.forEach((shipLayer) => {
      let step = 0;
      let forward = 1;
      const ship = ships.find((_ship) => _ship.id === shipLayer.customId);
      if(ship.positions.length > 1) {
        setInterval(() => {
          shipLayer.setLatLng(
            new LatLng(ship.positions[step][0], ship.positions[step][1])
          );
          step += forward;
          if (step >= ship.positions.length -1) {
            forward = -1
          }
          if(step == 0) {
            forward = 1;
          }
        }, 10000);
      }

    });
  }

  removeShips(): void {
    for (const shipLayer of this.shipsLayers) {
      this.map.removeLayer(shipLayer);
    }
    this.shipsLayers = [];
  }

  addDestination(
    destination: number[],
    origin: Coordinates,
    incident?: number[]
  ): void {
    const destinationMarker = L.marker(
      new LatLng(destination[0], destination[1])
    ).setIcon(
      icon({
        iconSize: [50, 50],
        iconAnchor: [50, 50],
        iconUrl: MarkerColor.YELLOW,
      })
    ) as CustomMarker;

    const point: LatLngExpression[] = [
      new LatLng(origin.lat, origin.lng),
      new LatLng(destination[0], destination[1]),
    ];
    const connection = new L.Polyline(point, {
      color: '#00BDBD',
      weight: 2,
      opacity: 0.9,
      smoothFactor: 0.2,
      className: 'additonal-marker-destination',
    });

    connection.addTo(this.map);
    destinationMarker.customId = 'additonal-marker-destination';

    this.map.addLayer(destinationMarker);
    let incidentMarker;
    if (incident) {
      incidentMarker = L.marker(new LatLng(incident[0], incident[1])).setIcon(
        icon({
          iconSize: [30, 30],
          iconUrl: MarkerColor.INCIDENT,
          className: 'incident-marker',
        })
      ) as CustomMarker;

      incidentMarker.customId = 'additonal-marker-destination';

      this.map.addLayer(incidentMarker);
    }
    this.centerMapBounds(false)
    this.destinationLayers = {
      destinationMarker,
      connection,
      incident: incidentMarker,
    };
  }

  public restoreMap(disabledInteractions?: boolean): void {
    this.markerSelected = undefined;
    this.idMarker = '';
    if (disabledInteractions) this.enabledAllInteractions();
    this.removeOpacity();
  }

  private initializeMap(): void {
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

  private addMarker(
    city: string,
    idDistributionCenter: number,
    type: CenterType,
    point: LatLng,
    iconType: string
  ): void {
    const marker = L.marker(point)
      .setIcon(
        icon({
          iconSize: [50, 50],
          iconAnchor: [50, 50],
          iconUrl: iconType,
        })
      )
      .on('mouseout', (ev) => {
        if (this.markerSelected?.target) return;
        this.removeTooltip();
        this.removeOpacity();
      })
      .on('mouseover', (ev: any) => {
        if (this.markerSelected?.target) return;
        if (!this.containerParent)
          this.containerParent = ev.originalEvent.target.parentElement;
        this.addToltip(ev.target.customId, ev);
        this.addOpacity();
      })
      .on('click', (marker: any) => {
        if (marker.target.type === CenterType.DESTINATION) {
          return;
        }
        this.clickedOnMarker = true;
        this.idMarker = idDistributionCenter.toString();
        this.containerParent = marker.force
          ? marker.target._icon.parentElement
          : marker.originalEvent.target.parentElement;
        this.clickMarker(marker);
      }) as CustomMarker;

    marker.customId = idDistributionCenter.toString();
    marker.city = city;
    marker.idDistributionCenter = idDistributionCenter;
    marker.type = type;
    this.initialMap.markers.push(marker);
    this.map.addLayer(marker);
  }

  test(): void {
    this.fireClick(this.idMarker);
  }

  fireClick(id: string): void {
    const marker = this.initialMap.markers.find((marker) => {
      return marker.customId === id;
    });
    marker?.fireEvent('click', { force: true, latlng: marker.getLatLng() });
  }

  clickMarker(marker: L.LeafletEvent) {
    this.zone.run(() => {
      if (this.detailsTrucksShowed) {
        this.truckClicked.emit(marker);
        this.disabledAllInteractions();
        this.addBorderSelected();
        this.markerSelected = marker;
      } else if (!this.markerSelected?.target) {
        this.removeTooltip();
        this.markerSelected = marker;
        this.disabledAllInteractions();
        this.isEndFlyTo = true;
        this.map.flyTo((marker as any).latlng, 7, { duration: 1 });
        this.markerClick.emit(marker);
      }
    });
  }

  private enabledAllInteractions(): void {
    this.disabledInteractions = false;
    this.map.dragging.enable();
    this.map.touchZoom.enable();
    this.map.doubleClickZoom.enable();
    this.map.scrollWheelZoom.enable();
    this.map.boxZoom.enable();
    this.map.boxZoom.enable();
    this.map.keyboard.enable();
    if (this.map.tap) this.map.tap.enable();
  }

  private disabledAllInteractions(): void {
    this.disabledInteractions = true;
    this.map.dragging.disable();
    this.map.touchZoom.disable();
    this.map.doubleClickZoom.disable();
    this.map.scrollWheelZoom.disable();
    this.map.boxZoom.disable();
    this.map.keyboard.disable();
    if (this.map.tap) this.map.tap.disable();
  }

  private addBorderSelected() {
    try {
    let markerSelected: any;
    this.map.eachLayer((layer: any) => {
      if (layer.customId == this.idMarker) {
        markerSelected = layer;
        return;
      }
    });
    this.removeTooltip();

    this.renderer.insertBefore(
      this.containerParent,
      this.borderSelected,
      markerSelected._icon
    );
    this.renderer.setAttribute(
      this.borderSelected,
      'style',
      markerSelected._icon.style.cssText
    );
    
      this.renderer.removeClass(markerSelected._icon, 'opacity-2');
    } catch {}
  }

  private addOpacity(): void {
    const elementToOpacity = this.elem.nativeElement.querySelectorAll(
      '.leaflet-interactive'
    );
    elementToOpacity.forEach((layer: any) => {
      this.renderer.addClass(layer, 'opacity-2');
    });
  }

  removeOpacity(): void {
    const elementToOpacity = this.elem.nativeElement.querySelectorAll(
      '.leaflet-interactive'
    );
    elementToOpacity.forEach((layer: any) => {
      this.renderer.removeClass(layer, 'opacity-2');
    });
  }

  private removeTooltip() {
    try {
      this.renderer.selectRootElement(
        '.custom-tooltip-map-name-border-dos',
        true
      );
      this.renderer.removeChild(this.containerParent, this.tooltip);
    } catch {
      return;
    }
  }

  private addToltip(id: string, marker: any) {
    if (marker.target.city === 'Magic Place') {
      this.renderer.addClass(this.tooltipName, 'black');
      this.renderer.setAttribute(
        this.tooltipLine,
        'src',
        'assets/images/map/border_city_mp.svg'
      );
    } else {
      this.renderer.setAttribute(
        this.tooltipLine,
        'src',
        'assets/images/map/border_city.svg'
      );
      this.renderer.removeClass(this.tooltipName, 'black');
    }
    this.renderer.insertBefore(
      marker.originalEvent.target.parentElement,
      this.tooltip,
      marker.originalEvent.target
    );
    this.renderer.setAttribute(
      this.tooltip,
      'style',
      marker.target._icon.style.cssText
    );
    this.renderer.setProperty(
      this.tooltipName,
      'innerText',
      marker.target.city
    );
  }

  private paintConnection(conections: Connection[]) {
    for (const conection of conections) {
      this.addConection([conection.startPoint, conection.endPoint]);
    }
    if (this.isNecesaryAddOpacity) {
      this.addOpacity();
      this.isNecesaryAddOpacity = false;
    }

    if (this.isNecesaryCenterBounds) {
      this.centerMapBounds();
      this.isNecesaryCenterBounds = false;
    }
  }

  private addConection(points: LatLngExpression[]): void {
    const newLine = new L.Polyline(points, {
      color: '#00BDBD',
      weight: 2,
      opacity: 0.9,
      smoothFactor: 0.2,
    });
    this.initialMap.connections.push(newLine);
    newLine.addTo(this.map);
  }
}
