import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyAttendanceHistoryComponent } from './monthly-attendance-history.component';

describe('MonthlyAttendanceHistoryComponent', () => {
  let component: MonthlyAttendanceHistoryComponent;
  let fixture: ComponentFixture<MonthlyAttendanceHistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MonthlyAttendanceHistoryComponent]
    });
    fixture = TestBed.createComponent(MonthlyAttendanceHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
