import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDonation } from './user-donation';

describe('UserDonation', () => {
  let component: UserDonation;
  let fixture: ComponentFixture<UserDonation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserDonation],
    }).compileComponents();

    fixture = TestBed.createComponent(UserDonation);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
