import { SharedModule } from './../shared/shared.module';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { MapsdRoutingModule } from './maps-routing.module';
import { DetailDistributionCenterModalComponent } from './map2d/detail-distribution-center-modal/detail-distribution-center-modal.component';
import { DetailVehicleModalComponent } from './map2d/detail-vehicle-modal/detail-vehicle-modal.component';
import { MapComponent } from './map2d/map/map.component';
import { Map2dComponent } from './map2d/map2d.component';
import { TruckIncidentModalComponent } from './map2d/truck-incident-modal/truck-incident-modal.component';
import { TruckComponent } from './map2d/truck/truck.component';
import { StatisticsComponent } from './map2d/statistics/statistics.component';
import { MagicPlacesComponent } from './map2d/magic-places/magic-places.component';
import { ReactiveFormsModule } from '@angular/forms';
import { GenericProblemModalComponent } from './map2d/generic-problem-modal/generic-problem-modal.component';
import { RouteDetailedComponent } from './map2d/route-detailed/route-detailed.component';
import { NgxEchartsModule } from 'ngx-echarts';
import { TruckReducedInfoModalComponent } from './map2d/truck-reduced-info-modal/truck-reduced-info-modal.component';
@NgModule({
  declarations: [
    Map2dComponent,
    MapComponent,
    DetailDistributionCenterModalComponent,
    DetailVehicleModalComponent,
    TruckIncidentModalComponent,
    TruckReducedInfoModalComponent,
    GenericProblemModalComponent,
    TruckComponent,
    StatisticsComponent,
    MagicPlacesComponent,
    RouteDetailedComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MapsdRoutingModule,
    LeafletModule,
    SharedModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts'),
    }),
  ],
})
export class MapsModule {}
