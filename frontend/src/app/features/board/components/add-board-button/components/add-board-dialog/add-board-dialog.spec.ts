import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBoardDialog } from './add-board-dialog';

describe('AddBoardDialog', () => {
  let component: AddBoardDialog;
  let fixture: ComponentFixture<AddBoardDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBoardDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddBoardDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
