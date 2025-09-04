import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskLogs } from './task-logs';

describe('TaskLogs', () => {
  let component: TaskLogs;
  let fixture: ComponentFixture<TaskLogs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskLogs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskLogs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
