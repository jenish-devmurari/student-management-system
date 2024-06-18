import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendanceHistoryComponent } from './attendance-history.component';

describe('AttendanceHistoryComponent', () => {
  let component: AttendanceHistoryComponent;
  let fixture: ComponentFixture<AttendanceHistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AttendanceHistoryComponent]
    });
    fixture = TestBed.createComponent(AttendanceHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
