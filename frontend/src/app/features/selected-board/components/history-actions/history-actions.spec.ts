import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryActions } from './history-actions';

describe('HistoryActions', () => {
  let component: HistoryActions;
  let fixture: ComponentFixture<HistoryActions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HistoryActions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistoryActions);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
