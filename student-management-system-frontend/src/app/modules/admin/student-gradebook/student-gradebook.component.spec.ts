import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentGradebookComponent } from './student-gradebook.component';

describe('StudentGradebookComponent', () => {
  let component: StudentGradebookComponent;
  let fixture: ComponentFixture<StudentGradebookComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StudentGradebookComponent]
    });
    fixture = TestBed.createComponent(StudentGradebookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
