import { TestBed } from '@angular/core/testing';

import { ConferenceService } from './conference-service.service';
import { UserService } from "./user.service";
import { GlobalsService } from "./globals.service";

describe('ConferenceService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: ConferenceService;
  let userService: UserService;
  let globalsService: GlobalsService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    globalsService = new GlobalsService();
    userService = new UserService(globalsService, httpClientSpy as any, '');
    service = new ConferenceService(httpClientSpy as any, userService, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
