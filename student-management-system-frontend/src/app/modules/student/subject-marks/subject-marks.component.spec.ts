import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectMarksComponent } from './subject-marks.component';

describe('SubjectMarksComponent', () => {
  let component: SubjectMarksComponent;
  let fixture: ComponentFixture<SubjectMarksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SubjectMarksComponent]
    });
    fixture = TestBed.createComponent(SubjectMarksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
