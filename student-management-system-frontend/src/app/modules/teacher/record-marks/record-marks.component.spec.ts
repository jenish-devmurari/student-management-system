import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecordMarksComponent } from './record-marks.component';

describe('RecordMarksComponent', () => {
  let component: RecordMarksComponent;
  let fixture: ComponentFixture<RecordMarksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RecordMarksComponent]
    });
    fixture = TestBed.createComponent(RecordMarksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
