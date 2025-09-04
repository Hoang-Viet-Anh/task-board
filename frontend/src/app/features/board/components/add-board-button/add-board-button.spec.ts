import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBoardButton } from './add-board-button';

describe('AddBoardButton', () => {
  let component: AddBoardButton;
  let fixture: ComponentFixture<AddBoardButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBoardButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddBoardButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
