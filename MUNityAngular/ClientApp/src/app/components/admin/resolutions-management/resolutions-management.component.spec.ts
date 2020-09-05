import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResolutionsManagementComponent } from './resolutions-management.component';

describe('ResolutionsManagementComponent', () => {
  let component: ResolutionsManagementComponent;
  //let fixture: ComponentFixture<ResolutionsManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResolutionsManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(ResolutionsManagementComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new ResolutionsManagementComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
