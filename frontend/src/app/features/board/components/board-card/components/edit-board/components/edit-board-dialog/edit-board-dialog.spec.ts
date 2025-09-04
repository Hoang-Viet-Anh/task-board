import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditBoardDialog } from './edit-board-dialog';

describe('EditBoardDialog', () => {
  let component: EditBoardDialog;
  let fixture: ComponentFixture<EditBoardDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditBoardDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditBoardDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
