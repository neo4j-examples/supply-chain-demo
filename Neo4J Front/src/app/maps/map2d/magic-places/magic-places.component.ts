import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { DistributionCenterService } from '../../services/map/distribution-center.service';

@Component({
  selector: 'neo4j-magic-places',
  templateUrl: './magic-places.component.html',
  styleUrls: ['./magic-places.component.scss'],
})
export class MagicPlacesComponent implements OnInit {
  @Input()
  private _itemsMagicPlaces: any = [];
  get itemsMagicPlaces() {
    return this._itemsMagicPlaces;
  }
  @Input() set itemsMagicPlaces(items: any) {
    for (const item of items) {
      this.magicPlaceForm.addControl(item.id, new FormControl(false));
    }
    this._itemsMagicPlaces = items;
  }
  @Output() getMagicPlaces = new EventEmitter<void>();
  magicPlaceForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private distributionCenterService: DistributionCenterService
  ) {
    this.magicPlaceForm = this.fb.group({});
  }

  ngOnInit(): void {
    this.magicPlaceForm.valueChanges.subscribe((res: any) => {
      this.sendMagicPlaceParams();
    });
  }

  sendMagicPlaceParams(): void {
    this.getMagicPlaces.emit(this.magicPlaceForm.value);
  }
}
