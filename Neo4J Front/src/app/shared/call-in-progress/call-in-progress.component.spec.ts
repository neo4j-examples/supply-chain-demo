import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallInProgressComponent } from './call-in-progress.component';

describe('CallInProgressComponent', () => {
  let component: CallInProgressComponent;
  let fixture: ComponentFixture<CallInProgressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CallInProgressComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CallInProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
