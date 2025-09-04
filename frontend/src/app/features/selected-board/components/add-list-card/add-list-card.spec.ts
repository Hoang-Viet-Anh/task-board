import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddListCard } from './add-list-card';

describe('AddListCard', () => {
  let component: AddListCard;
  let fixture: ComponentFixture<AddListCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddListCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddListCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
