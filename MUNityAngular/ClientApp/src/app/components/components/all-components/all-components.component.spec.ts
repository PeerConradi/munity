import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllComponentsComponent } from './all-components.component';

describe('AllComponentsComponent', () => {
  let component: AllComponentsComponent;
  let fixture: ComponentFixture<AllComponentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllComponentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllComponentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
