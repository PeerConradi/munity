import { TestBed } from '@angular/core/testing';

import { ConferenceService } from './conference-service.service';

describe('ConferenceServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ConferenceService = TestBed.get(ConferenceService);
    expect(service).toBeTruthy();
  });
});
