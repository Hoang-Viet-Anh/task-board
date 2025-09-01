import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveBoard } from './remove-board';

describe('RemoveBoard', () => {
  let component: RemoveBoard;
  let fixture: ComponentFixture<RemoveBoard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveBoard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemoveBoard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
