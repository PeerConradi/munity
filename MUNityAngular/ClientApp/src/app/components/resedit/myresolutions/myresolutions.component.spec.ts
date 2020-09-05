import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyresolutionsComponent } from './myresolutions.component';

describe('MyresolutionsComponent', () => {
  let component: MyresolutionsComponent;
  //let fixture: ComponentFixture<MyresolutionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyresolutionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(MyresolutionsComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new MyresolutionsComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
