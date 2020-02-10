import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExploreConferencesComponent } from './explore-conferences.component';

describe('ExploreConferencesComponent', () => {
  let component: ExploreConferencesComponent;
  let fixture: ComponentFixture<ExploreConferencesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExploreConferencesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExploreConferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
