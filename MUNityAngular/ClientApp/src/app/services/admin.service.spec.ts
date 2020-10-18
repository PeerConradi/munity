import { TestBed } from '@angular/core/testing';

import { AdminService } from './admin.service';
import { UserService } from "./user.service";
import { GlobalsService } from "./globals.service";

describe('AdminService', () => {
  let service: AdminService;
  let httpClientSpy: { get: jasmine.Spy };
  let userService: UserService;
  let globalService: GlobalsService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    globalService = new GlobalsService();
    userService = new UserService(globalService, httpClientSpy as any, '');

    service = new AdminService(httpClientSpy as any, userService as any, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
