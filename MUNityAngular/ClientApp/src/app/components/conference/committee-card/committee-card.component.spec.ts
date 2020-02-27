import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommitteeCardComponent } from './committee-card.component';

describe('CommitteeCardComponent', () => {
  let component: CommitteeCardComponent;
  let fixture: ComponentFixture<CommitteeCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommitteeCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommitteeCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
