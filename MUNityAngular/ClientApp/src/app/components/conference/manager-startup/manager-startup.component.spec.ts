import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerStartupComponent } from './manager-startup.component';

describe('ManagerStartupComponent', () => {
  let component: ManagerStartupComponent;
  let fixture: ComponentFixture<ManagerStartupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagerStartupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagerStartupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
