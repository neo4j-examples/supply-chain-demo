import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { DistributionCenter } from '../../models/distribution-center.model';

@Component({
  selector: 'neo4j-detail-vehicle-modal',
  templateUrl: './detail-vehicle-modal.component.html',
  styleUrls: ['./detail-vehicle-modal.component.scss'],
})
export class DetailVehicleModalComponent implements OnInit, AfterViewInit {
  @Output() closeModalTruck = new EventEmitter<string>();
  @Output() showIncident = new EventEmitter<string>();
  @Output() showRouteDetailed = new EventEmitter<string>();
  @Output() hiddenRouteDetailed = new EventEmitter<void>();
  @Input() details: DistributionCenter;
  @Input() route: any;
  isShowedRoute: boolean;
  actualDate = new Date();
  ngOnInit(): void {}

  ngAfterViewInit(): void {}

  clickClose(): void {
    this.isShowedRoute = false;
    this.closeModalTruck.emit();
    this.hiddenRouteDetailed.emit();
  }

  showIncidentClick(): void {
    this.showIncident.emit();
  }

  showRoute(): void {
    if (!this.isShowedRoute) {
      this.isShowedRoute = true;
      this.showRouteDetailed.emit();
    } else {
      this.isShowedRoute = false;
      this.hiddenRouteDetailed.emit();
    }
  }
}
