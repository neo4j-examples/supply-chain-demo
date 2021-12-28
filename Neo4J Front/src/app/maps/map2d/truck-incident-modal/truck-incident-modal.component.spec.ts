import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TruckIncidentModalComponent } from './truck-incident-modal.component';

describe('TruckIncidentModalComponent', () => {
  let component: TruckIncidentModalComponent;
  let fixture: ComponentFixture<TruckIncidentModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TruckIncidentModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TruckIncidentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
