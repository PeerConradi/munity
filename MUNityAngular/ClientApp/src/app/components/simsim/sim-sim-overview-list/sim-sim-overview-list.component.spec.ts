import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimOverviewListComponent } from './sim-sim-overview-list.component';

describe('SimSimOverviewListComponent', () => {
  let component: SimSimOverviewListComponent;
  //let fixture: ComponentFixture<SimSimOverviewListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimOverviewListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(SimSimOverviewListComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new SimSimOverviewListComponent(null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
