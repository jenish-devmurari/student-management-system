import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GradebookComponent } from './gradebook.component';

describe('GradebookComponent', () => {
  let component: GradebookComponent;
  let fixture: ComponentFixture<GradebookComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GradebookComponent]
    });
    fixture = TestBed.createComponent(GradebookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
