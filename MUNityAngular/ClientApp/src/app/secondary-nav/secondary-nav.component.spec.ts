import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SecondaryNavComponent } from './secondary-nav.component';

describe('SecondaryNavComponent', () => {
  let component: SecondaryNavComponent;
  let fixture: ComponentFixture<SecondaryNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SecondaryNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SecondaryNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
