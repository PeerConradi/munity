import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimLobbyElementComponent } from './sim-sim-lobby-element.component';

describe('SimSimLobbyElementComponent', () => {
  let component: SimSimLobbyElementComponent;
  let fixture: ComponentFixture<SimSimLobbyElementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimLobbyElementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimSimLobbyElementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
