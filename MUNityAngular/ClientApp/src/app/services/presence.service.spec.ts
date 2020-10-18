import { TestBed } from '@angular/core/testing';

import { PresenceService } from './presence.service';
import { UserService } from "./user.service";
import { GlobalsService } from "./globals.service";

describe('PresenceService', () => {

  let httpClientSpy: { get: jasmine.Spy };
  let service: PresenceService;
  let userService: UserService;
  let globalsService: GlobalsService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    globalsService = new GlobalsService();
    userService = new UserService(globalsService, httpClientSpy as any, '');
    service = new PresenceService(userService, httpClientSpy as any, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
