import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GradebookHistoryComponent } from './gradebook-history.component';

describe('GradebookHistoryComponent', () => {
  let component: GradebookHistoryComponent;
  let fixture: ComponentFixture<GradebookHistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GradebookHistoryComponent]
    });
    fixture = TestBed.createComponent(GradebookHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
