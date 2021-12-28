import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import Chart from 'chart.js/auto';
import { DistributionCenter } from '../../models/distribution-center.model';
import { DistributionCenterService } from '../../services/map/distribution-center.service';

@Component({
  selector: 'neo4j-detail-distribution-center-modal',
  templateUrl: './detail-distribution-center-modal.component.html',
  styleUrls: ['./detail-distribution-center-modal.component.scss'],
})
export class DetailDistributionCenterModalComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  @Output() getDetailsTrucks = new EventEmitter<string>();
  @Output() hiddenDetailsTrucks = new EventEmitter<string>();
  @Input() details: DistributionCenter;
  @Output() paintMagicPlaces = new EventEmitter<void>();
  wharehouseCapacity: any[] = [];
  detailsShowed: any;
  showStadistics = true;
  planningCtx: any;
  itemsMagicPlaces = [];
  magicPlacesShowed: boolean;
  minimize: boolean = false;

  constructor(
    private readonly distributionCenterService: DistributionCenterService
  ) {}

  ngOnInit(): void {
    this.detailsShowed = JSON.parse(
      localStorage.getItem('detailsShowed') || 'false'
    );
    const capacity =
      (this.details.warehouseAndDistribution * 23) /
      this.details.warehouseAndDistributionTotal;
    for (let i = 0; i < 24; i++) {
      if (capacity < i) {
        this.wharehouseCapacity.push({ capacity: false });
      } else {
        this.wharehouseCapacity.push({ capacity: true });
      }
    }
  }

  clickClose(): void {
    this.close.emit();
  }

  showMagicPlacesForm(): void {
    if (!this.magicPlacesShowed) {
      this.distributionCenterService
        .getVariablesForMagicPlaces()
        .subscribe((res: any) => {
          this.itemsMagicPlaces = res;
          this.showStadistics = !this.showStadistics;
          this.detailsShowed = false;
          this.magicPlacesShowed = true;
        });
    } else {
      this.hiddenDetailsTrucks.emit(this.details.id);
      this.showStadistics = true;
      this.magicPlacesShowed = false;
    }
  }

  showDetailsTrucks(): void {
    this.showStadistics = true;
    if (!this.detailsShowed) {
      this.getDetailsTrucks.emit(this.details.id);
      this.detailsShowed = true;
      this.magicPlacesShowed = false;
    } else {
      this.hiddenDetailsTrucks.emit(this.details.id);
      this.detailsShowed = false;
    }

    localStorage.setItem('detailsShowed', this.detailsShowed);
  }

  onGetMagicPlaces(event: any): void {
    const features = {
      idDistributionCenter: this.details.id,
      features: event,
    };
    this.distributionCenterService.getMagicPlaces(features).subscribe((res) => {
      this.paintMagicPlaces.emit(res);
    });
  }
}
