import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskListSelect } from './task-list-select';

describe('TaskListSelect', () => {
  let component: TaskListSelect;
  let fixture: ComponentFixture<TaskListSelect>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskListSelect]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskListSelect);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
