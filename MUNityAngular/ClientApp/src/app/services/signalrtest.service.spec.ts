import { TestBed } from '@angular/core/testing';

import { SignalrtestService } from './signalrtest.service';

describe('SignalrtestService', () => {
  let service: SignalrtestService;

  beforeEach(() => {
    service = new SignalrtestService('');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
