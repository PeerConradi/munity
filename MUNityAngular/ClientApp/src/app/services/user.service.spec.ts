import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';


describe('UserService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: UserService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new UserService(httpClientSpy as any, '');
  }
  );

  it('should be created', () => {
    //const service: UserService = TestBed.get(UserService);
    expect(service).toBeTruthy();
  });
});
