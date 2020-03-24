import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditConferenceLayoutComponent } from './edit-conference-layout.component';

describe('EditConferenceLayoutComponent', () => {
  let component: EditConferenceLayoutComponent;
  let fixture: ComponentFixture<EditConferenceLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditConferenceLayoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditConferenceLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
