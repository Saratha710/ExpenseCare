import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDonations } from './user-donations';

describe('UserDonations', () => {
  let component: UserDonations;
  let fixture: ComponentFixture<UserDonations>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserDonations],
    }).compileComponents();

    fixture = TestBed.createComponent(UserDonations);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
