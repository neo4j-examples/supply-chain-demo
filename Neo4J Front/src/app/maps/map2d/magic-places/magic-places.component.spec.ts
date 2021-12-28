import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MagicPlacesComponent } from './magic-places.component';

describe('MagicPlacesComponent', () => {
  let component: MagicPlacesComponent;
  let fixture: ComponentFixture<MagicPlacesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MagicPlacesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MagicPlacesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
