import { SearcherService } from './../../core/services/searcher.service';
import { Connection } from 'src/app/maps/models/connection.model';
import { DistributionCenterService } from './../services/map/distribution-center.service';
import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Map3dService } from '../services/map3d/map3d.service';
import { LocationMap } from '../models/location.model';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'neo4j-map3d',
  templateUrl: './map3d.component.html',
  styleUrls: ['./map3d.component.scss'],
})
export class Map3dComponent implements OnInit, AfterViewInit {
  locations: LocationMap[];
  connections: Connection[];
  unsubscribe = new Subject<void>();

  constructor(
    private readonly map3dService: Map3dService,
    private readonly distributionCenterService: DistributionCenterService,
    private readonly searcherService: SearcherService
  ) {}

  ngOnInit(): void {
   
  }

  ngOnDestroy(): void {
   this.unsubscribe.next();
  }

  ngAfterViewInit(): void {
    this.map3dService.earthClicked.pipe(takeUntil(this.unsubscribe)).subscribe(() => {
      this.map3dService.showLines(this.connections);
      this.map3dService.addMarkers(this.locations);
    });
    this.distributionCenterService
      .getLocations()
      .subscribe(({ locations, connections }) => {
        this.locations = locations;
        this.connections = connections;
        this.map3dService.initMap3D();
      });

      this.searcherService.searched.subscribe((location) => {
        this.map3dService.flyTo(location[0], location[1]);
      });
  }
}
