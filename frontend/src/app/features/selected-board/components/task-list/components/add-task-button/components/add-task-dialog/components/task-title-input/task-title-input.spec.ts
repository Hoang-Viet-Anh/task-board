import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTitleInput } from './task-title-input';

describe('TaskTitleInput', () => {
  let component: TaskTitleInput;
  let fixture: ComponentFixture<TaskTitleInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskTitleInput]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskTitleInput);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
