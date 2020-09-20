import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDelegationRowComponent } from './edit-delegation-row.component';

describe('EditDelegationRowComponent', () => {
  let component: EditDelegationRowComponent;
  let fixture: ComponentFixture<EditDelegationRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditDelegationRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditDelegationRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
