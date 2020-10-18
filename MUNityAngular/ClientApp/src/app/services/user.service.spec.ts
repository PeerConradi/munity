import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { GlobalsService } from "./globals.service";


describe('UserService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: UserService;
  let globalsService: GlobalsService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    globalsService = new GlobalsService();
    service = new UserService(globalsService, httpClientSpy as any, '');
  }
  );

  it('should be created', () => {
    //const service: UserService = TestBed.get(UserService);
    expect(service).toBeTruthy();
  });
});
