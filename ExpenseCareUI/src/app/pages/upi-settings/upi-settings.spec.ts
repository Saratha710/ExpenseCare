import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpiSettings } from './upi-settings';

describe('UpiSettings', () => {
  let component: UpiSettings;
  let fixture: ComponentFixture<UpiSettings>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpiSettings],
    }).compileComponents();

    fixture = TestBed.createComponent(UpiSettings);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
