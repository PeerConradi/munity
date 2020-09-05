import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResViewComponent } from './res-view.component';
import { ActivatedRoute } from '@angular/router';
import { Observable, of } from 'rxjs';
import { ResolutionService } from "../../../services/resolution.service";
import { HttpClient } from '@angular/common/http';
import { UserService } from "../../../services/user.service";

describe('ResViewComponent', () => {
  let component: ResViewComponent;
  //let fixture: ComponentFixture<ResViewComponent>;
  let userService: UserService;
  let resolutionService: ResolutionService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ResViewComponent],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of([{ id: 1 }]),
          },
        },
        HttpClient,
        UserService,
        ResolutionService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(ResViewComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    var fakeRoute: ActivatedRoute = new ActivatedRoute();
    fakeRoute.params = of([{ id: 1 }]);
    resolutionService = new ResolutionService(null, null, null, '');
    let spy = spyOn(resolutionService, 'getResolution').and.returnValue(null);
    component = new ResViewComponent(resolutionService, fakeRoute);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
