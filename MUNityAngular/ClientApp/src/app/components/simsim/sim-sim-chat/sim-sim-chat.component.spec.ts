import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimChatComponent } from './sim-sim-chat.component';

describe('SimSimChatComponent', () => {
  let component: SimSimChatComponent;
  let fixture: ComponentFixture<SimSimChatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimChatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimSimChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
