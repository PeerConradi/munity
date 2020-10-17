import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyOrganisationsComponent } from './my-organisations.component';

describe('MyOrganisationsComponent', () => {
  let component: MyOrganisationsComponent;
  let fixture: ComponentFixture<MyOrganisationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyOrganisationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyOrganisationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
