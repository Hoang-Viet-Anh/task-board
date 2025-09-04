import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskDateSelect } from './task-date-select';

describe('TaskDateSelect', () => {
  let component: TaskDateSelect;
  let fixture: ComponentFixture<TaskDateSelect>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskDateSelect]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskDateSelect);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
