import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskPrioritySelect } from './task-priority-select';

describe('TaskPrioritySelect', () => {
  let component: TaskPrioritySelect;
  let fixture: ComponentFixture<TaskPrioritySelect>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskPrioritySelect]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskPrioritySelect);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
