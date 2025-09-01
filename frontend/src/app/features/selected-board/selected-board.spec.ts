import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectedBoard } from './selected-board';

describe('SelectedBoard', () => {
  let component: SelectedBoard;
  let fixture: ComponentFixture<SelectedBoard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelectedBoard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectedBoard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
