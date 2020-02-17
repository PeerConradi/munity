import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DelegationDetailsComponent } from './delegation-details.component';

describe('DelegationDetailsComponent', () => {
  let component: DelegationDetailsComponent;
  let fixture: ComponentFixture<DelegationDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DelegationDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DelegationDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
