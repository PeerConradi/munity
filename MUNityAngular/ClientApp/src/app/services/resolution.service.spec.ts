import { TestBed } from '@angular/core/testing';

import { ResolutionService } from './resolution.service';

describe('ResolutionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ResolutionService = TestBed.get(ResolutionService);
    expect(service).toBeTruthy();
  });
});
