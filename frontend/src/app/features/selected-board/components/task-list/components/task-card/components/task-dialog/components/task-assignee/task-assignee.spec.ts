import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskAssignee } from './task-assignee';

describe('TaskAssignee', () => {
  let component: TaskAssignee;
  let fixture: ComponentFixture<TaskAssignee>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskAssignee]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskAssignee);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
