import { TestBed } from '@angular/core/testing';

import { AdminService } from './admin.service';
import { UserService } from "./user.service";

describe('AdminService', () => {
  let service: AdminService;
  let httpClientSpy: { get: jasmine.Spy };
  let userService: UserService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    userService = new UserService(httpClientSpy as any, '');
    service = new AdminService(httpClientSpy as any, userService as any, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
