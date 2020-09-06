import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConferenceTodoListWidgetComponent } from './conference-todo-list-widget.component';

describe('ConferenceTodoListWidgetComponent', () => {
  let component: ConferenceTodoListWidgetComponent;
  let fixture: ComponentFixture<ConferenceTodoListWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConferenceTodoListWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConferenceTodoListWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
