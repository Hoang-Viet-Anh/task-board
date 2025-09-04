import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeListDropdown } from './change-list-dropdown';

describe('ChangeListDropdown', () => {
  let component: ChangeListDropdown;
  let fixture: ComponentFixture<ChangeListDropdown>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangeListDropdown]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeListDropdown);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
