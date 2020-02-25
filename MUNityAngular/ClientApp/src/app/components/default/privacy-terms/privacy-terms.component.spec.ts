import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyTermsComponent } from './privacy-terms.component';

describe('PrivacyTermsComponent', () => {
  let component: PrivacyTermsComponent;
  let fixture: ComponentFixture<PrivacyTermsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrivacyTermsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyTermsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
