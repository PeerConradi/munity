import { TestBed } from '@angular/core/testing';

import { ConferenceService } from './conference-service.service';
import { UserService } from "./user.service";

describe('ConferenceService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: ConferenceService;
  let userService: UserService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    userService = new UserService(httpClientSpy as any, '');
    service = new ConferenceService(httpClientSpy as any, userService, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
