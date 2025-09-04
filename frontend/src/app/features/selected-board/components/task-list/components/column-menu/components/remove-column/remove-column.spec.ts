import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveColumn } from './remove-column';

describe('RemoveColumn', () => {
  let component: RemoveColumn;
  let fixture: ComponentFixture<RemoveColumn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveColumn]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemoveColumn);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
