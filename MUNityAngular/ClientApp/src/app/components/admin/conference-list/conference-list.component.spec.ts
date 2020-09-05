import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConferenceListComponent } from './conference-list.component';

describe('ConferenceListComponent', () => {
  let component: ConferenceListComponent;
  //let fixture: ComponentFixture<ConferenceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConferenceListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(ConferenceListComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new ConferenceListComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
