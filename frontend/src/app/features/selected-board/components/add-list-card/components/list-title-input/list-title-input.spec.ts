import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTitleInput } from './list-title-input';

describe('ListTitleInput', () => {
  let component: ListTitleInput;
  let fixture: ComponentFixture<ListTitleInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListTitleInput]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListTitleInput);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
