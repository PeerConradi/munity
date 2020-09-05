import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyConferencesOverviewComponent } from './my-conferences-overview.component';

describe('MyConferencesOverviewComponent', () => {
  let component: MyConferencesOverviewComponent;
  //let fixture: ComponentFixture<MyConferencesOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyConferencesOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(MyConferencesOverviewComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new MyConferencesOverviewComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
