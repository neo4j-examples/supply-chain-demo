import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailDistributionCenterModalComponent } from './detail-distribution-center-modal.component';

describe('DetailDistributionCenterModalComponent', () => {
  let component: DetailDistributionCenterModalComponent;
  let fixture: ComponentFixture<DetailDistributionCenterModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetailDistributionCenterModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailDistributionCenterModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
