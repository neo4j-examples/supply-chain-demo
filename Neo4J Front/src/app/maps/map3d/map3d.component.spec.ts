import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Map3dComponent } from './map3d.component';

describe('Map3dComponent', () => {
  let component: Map3dComponent;
  let fixture: ComponentFixture<Map3dComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Map3dComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Map3dComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
