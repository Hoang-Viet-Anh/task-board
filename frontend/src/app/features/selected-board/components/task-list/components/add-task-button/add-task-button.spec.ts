import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTaskButton } from './add-task-button';

describe('AddTaskButton', () => {
  let component: AddTaskButton;
  let fixture: ComponentFixture<AddTaskButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddTaskButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddTaskButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
