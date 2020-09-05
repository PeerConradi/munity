import { TestBed } from '@angular/core/testing';

import { ResolutionService } from './resolution.service';
import { UserService } from "./user.service";

describe('ResolutionService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: ResolutionService;
  let userService: UserService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    userService = new UserService(httpClientSpy as any, '');
    service = new ResolutionService(httpClientSpy as any, userService, null, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
