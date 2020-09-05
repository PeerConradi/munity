import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSettingsComponent } from './account-settings.component';
import { FormBuilder } from '@angular/forms';

describe('AccountSettingsComponent', () => {
  let component: AccountSettingsComponent;
  //let fixture: ComponentFixture<AccountSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(AccountSettingsComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    let formBuild = new FormBuilder();
    component = new AccountSettingsComponent(null, formBuild, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
