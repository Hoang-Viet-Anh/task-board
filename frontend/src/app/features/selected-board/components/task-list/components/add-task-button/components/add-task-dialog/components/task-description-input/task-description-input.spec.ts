import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskDescriptionInput } from './task-description-input';

describe('TaskDescriptionInput', () => {
  let component: TaskDescriptionInput;
  let fixture: ComponentFixture<TaskDescriptionInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskDescriptionInput]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskDescriptionInput);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
