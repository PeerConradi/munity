import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceDelegationsComponent } from './manage-conference-delegations.component';

describe('ManageConferenceDelegationsComponent', () => {
  let component: ManageConferenceDelegationsComponent;
  let fixture: ComponentFixture<ManageConferenceDelegationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ManageConferenceDelegationsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // fixture = TestBed.createComponent(ManageConferenceDelegationsComponent);
    // component = fixture.componentInstance;
    // fixture.detectChanges();
    component = new ManageConferenceDelegationsComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
