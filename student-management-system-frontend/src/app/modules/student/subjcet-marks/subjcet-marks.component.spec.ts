import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjcetMarksComponent } from './subjcet-marks.component';

describe('SubjcetMarksComponent', () => {
  let component: SubjcetMarksComponent;
  let fixture: ComponentFixture<SubjcetMarksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SubjcetMarksComponent]
    });
    fixture = TestBed.createComponent(SubjcetMarksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
