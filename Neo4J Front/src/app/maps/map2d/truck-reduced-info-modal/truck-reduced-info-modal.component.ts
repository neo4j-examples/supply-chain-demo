import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LocationMap } from '../../models/location.model';

@Component({
  selector: 'neo4j-truck-reduced-info-modal',
  templateUrl: './truck-reduced-info-modal.component.html',
  styleUrls: ['./truck-reduced-info-modal.component.scss']
})
export class TruckReducedInfoModalComponent implements OnInit {

  @Input() locations: LocationMap[];
  @Output() truckClicked = new EventEmitter<any>();
  @Output() backToDetails = new EventEmitter<boolean>();
  constructor() { }

  ngOnInit() {
  }

}
