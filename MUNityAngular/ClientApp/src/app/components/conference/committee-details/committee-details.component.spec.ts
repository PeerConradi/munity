import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommitteeDetailsComponent } from './committee-details.component';

describe('CommitteeDetailsComponent', () => {
  let component: CommitteeDetailsComponent;
  let fixture: ComponentFixture<CommitteeDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommitteeDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommitteeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
