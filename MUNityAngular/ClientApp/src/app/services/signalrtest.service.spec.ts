import { TestBed } from '@angular/core/testing';

import { SignalrtestService } from './signalrtest.service';

describe('SignalrtestService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SignalrtestService = TestBed.get(SignalrtestService);
    expect(service).toBeTruthy();
  });
});
