import { TestBed } from '@angular/core/testing';

import { PresenceService } from './presence.service';
import { UserService } from "./user.service";

describe('PresenceService', () => {

  let httpClientSpy: { get: jasmine.Spy };
  let service: PresenceService;
  let userService: UserService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    userService = new UserService(httpClientSpy as any, '');
    service = new PresenceService(userService, httpClientSpy as any, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
