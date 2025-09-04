import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InviteCodeCard } from './invite-code-card';

describe('InviteCodeCard', () => {
  let component: InviteCodeCard;
  let fixture: ComponentFixture<InviteCodeCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InviteCodeCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InviteCodeCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
